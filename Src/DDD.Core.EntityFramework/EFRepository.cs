using EnsureThat;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Common;

namespace DDD.Core.Infrastructure.Data
{
    using Domain;
    using Application;
    using Mapping;
    using Threading;

    public abstract class EFRepository<TContext, TDomainEntity, TStateEntity, TIdentity> : IRepository<TDomainEntity, TIdentity>, IDisposable
        where TContext : DbBoundedContext
        where TDomainEntity : DomainEntity, IStateObjectConvertible<TStateEntity>
        where TStateEntity : class, IStateEntity, new()
        where TIdentity : ComparableValueObject
    {

        #region Fields

        private readonly IDbContextFactory<TContext> contextFactory;
        private readonly IObjectTranslator<TStateEntity, TDomainEntity> entityTranslator;
        private readonly IObjectTranslator<IEvent, Event> eventTranslator;
        private readonly IObjectTranslator<Exception, RepositoryException> exceptionTranslator = new EFRepositoryExceptionTranslator();
        private TContext context;
        private bool disposed;

        #endregion Fields

        #region Constructors

        protected EFRepository(IDbContextFactory<TContext> contextFactory,
                               IObjectTranslator<TStateEntity, TDomainEntity> entityTranslator,
                               IObjectTranslator<IEvent, Event> eventTranslator)
        {
            Ensure.That(contextFactory, nameof(contextFactory)).IsNotNull();
            Ensure.That(entityTranslator, nameof(entityTranslator)).IsNotNull();
            Ensure.That(eventTranslator, nameof(eventTranslator)).IsNotNull();
            this.contextFactory = contextFactory;
            this.entityTranslator = entityTranslator;
            this.eventTranslator = eventTranslator;
        }

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public TDomainEntity Find(TIdentity identity)
        {
            Ensure.That(identity, nameof(identity)).IsNotNull();
            try
            {
                var keyValues = identity.PrimitiveEqualityComponents();
                var stateEntity = this.Find(keyValues);
                return this.TranslateEntity(stateEntity);
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<RepositoryException>())
            {
                throw this.TranslateException(ex);
            }
        }

        public async Task<TDomainEntity> FindAsync(TIdentity identity, CancellationToken cancellationToken = default)
        {
            Ensure.That(identity, nameof(identity)).IsNotNull();
            try
            {
                await new SynchronizationContextRemover();
                var keyValues = identity.PrimitiveEqualityComponents();
                var stateEntity = await this.FindAsync(keyValues, cancellationToken);
                return this.TranslateEntity(stateEntity);
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<RepositoryException>())
            {
                throw this.TranslateException(ex);
            }
        }

        public void Save(TDomainEntity aggregate)
        {
            Ensure.That(aggregate, nameof(aggregate)).IsNotNull();
            try
            {
                var stateEntity = aggregate.ToState();
                var guidGenerator = this.GetConnection().SequentialGuidGenerator();
                var events = ToEvents(guidGenerator, aggregate);
                this.Save(stateEntity, events);
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<RepositoryException>())
            {
                throw this.TranslateException(ex);
            }
        }

        public async Task SaveAsync(TDomainEntity aggregate, CancellationToken cancellationToken = default)
        {
            Ensure.That(aggregate, nameof(aggregate)).IsNotNull();
            try
            {
                await new SynchronizationContextRemover();
                var stateEntity = aggregate.ToState();
                var guidGenerator = (await this.GetConnectionAsync(cancellationToken)).SequentialGuidGenerator();
                var events = ToEvents(guidGenerator, aggregate);
                await this.SaveAsync(stateEntity, events, cancellationToken);
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<RepositoryException>())
            {
                throw this.TranslateException(ex);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    this.context?.Dispose();
                disposed = true;
            }
        }

        protected virtual TStateEntity Find(IEnumerable<object> keyValues)
        {
            var keyNames = this.GetContext().GetKeyNames<TStateEntity>();
            if (keyValues.Count() != keyNames.Count())
                throw new InvalidOperationException($"You must specify {keyNames.Count()} identity components.");
            var expression = BuildFindExpression(keyNames, keyValues);
            return this.Query().FirstOrDefault(expression);
        }

        protected virtual async Task<TStateEntity> FindAsync(IEnumerable<object> keyValues, CancellationToken cancellationToken)
        {
            var keyNames = (await this.GetContextAsync(cancellationToken)).GetKeyNames<TStateEntity>();
            if (keyValues.Count() != keyNames.Count())
                throw new InvalidOperationException($"You must specify {keyNames.Count()} identity components.");
            var expression = BuildFindExpression(keyNames, keyValues);
            var query = await this.QueryAsync(cancellationToken);
            return await query.FirstOrDefaultAsync(expression, cancellationToken);
        }

        protected DbConnection GetConnection()
        {
            return this.GetContext().Database.GetDbConnection();
        }

        protected async Task<DbConnection> GetConnectionAsync(CancellationToken cancellationToken)
        {
            return (await this.GetContextAsync(cancellationToken)).Database.GetDbConnection();
        }

        protected IQueryable<TStateEntity> Query()
        {
            var query = this.GetContext().Set<TStateEntity>().AsNoTracking().AsQueryable();
            foreach (var path in this.RelatedEntitiesPaths())
                query = query.Include(path);
            return query;
        }

        protected async Task<IQueryable<TStateEntity>> QueryAsync(CancellationToken cancellationToken)
        {
            var query = (await this.GetContextAsync(cancellationToken)).Set<TStateEntity>().AsNoTracking().AsQueryable();
            foreach (var path in this.RelatedEntitiesPaths())
                query = query.Include(path);
            return query;
        }

        protected virtual IEnumerable<Expression<Func<TStateEntity, object>>> RelatedEntitiesPaths()
        {
            return Enumerable.Empty<Expression<Func<TStateEntity, object>>>();
        }

        protected virtual void Save(TStateEntity stateEntity, IEnumerable<Event> events)
        {
            var context = this.GetContext();
            context.Set<TStateEntity>().Add(stateEntity);
            context.Events.AddRange(events);
            context.SaveChanges();
        }

        protected virtual async Task SaveAsync(TStateEntity stateEntity, IEnumerable<Event> events, CancellationToken cancellationToken)
        {
            var context = await this.GetContextAsync(cancellationToken);
            context.Set<TStateEntity>().Add(stateEntity);
            context.Events.AddRange(events);
            await context.SaveChangesAsync(cancellationToken);
        }

        protected TDomainEntity TranslateEntity(TStateEntity stateEntity)
        {
            if (stateEntity == null) return null;
            return this.entityTranslator.Translate(stateEntity);
        }

        protected RepositoryException TranslateException(Exception ex)
        {
            return this.exceptionTranslator.Translate(ex, new { EntityType = typeof(TDomainEntity) });
        }

        private static Expression<Func<TStateEntity, bool>> BuildFindExpression(IEnumerable<string> keyNames,
                                                                                IEnumerable<object> keyValues)
        {
            var entity = Expression.Parameter(typeof(TStateEntity), "entity");
            Expression find = null;
            for (int i = 0; i < keyNames.Count(); i++)
            {
                var key = Expression.Property(entity, keyNames.ElementAt(i));
                var keyValue = Expression.Constant(keyValues.ElementAt(i));
                var equals = key.Type.GetMethod("Equals", new[] { key.Type });
                var keyEqualsKeyValue = Expression.Call(key, equals, keyValue);
                if (find == null)
                    find = keyEqualsKeyValue;
                else
                    find = Expression.AndAlso(find, keyEqualsKeyValue);
            }
            return Expression.Lambda<Func<TStateEntity, bool>>(find, entity);
        }

        private TContext GetContext()
        {
            if (this.context == null)
                this.context = this.contextFactory.CreateDbContext();
            return this.context;
        }

        private async Task<TContext> GetContextAsync(CancellationToken cancellationToken)
        {
            if (this.context == null)
                this.context = await this.contextFactory.CreateDbContextAsync(cancellationToken);
            return this.context;
        }
        private IEnumerable<Event> ToEvents(IValueGenerator<Guid> guidGenerator, TDomainEntity aggregate)
        {
            var username = Thread.CurrentPrincipal?.Identity?.Name;
            return aggregate.AllEvents().Select(e =>
            {
                var context = new
                {
                    EventId = guidGenerator.Generate(),
                    StreamId = aggregate.IdentityAsString(),
                    StreamType = aggregate.GetType().Name,
                    IssuedBy = username
                };
                return this.eventTranslator.Translate(e, context);
            });
        }

        #endregion Methods

    }
}