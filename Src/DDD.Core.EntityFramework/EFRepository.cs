using Conditions;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Common;
using DDD.Data;

namespace DDD.Core.Infrastructure.Data
{
    using Domain;
    using Application;
    using Mapping;
    using Threading;

    public abstract class EFRepository<TContext, TDomainEntity, TStateEntity, TIdentity>
        : IRepository<TDomainEntity, TIdentity>
        where TContext : BoundedContext
        where TDomainEntity : DomainEntity, IStateObjectConvertible<TStateEntity>
        where TStateEntity : class, IStateEntity, new()
        where TIdentity : ComparableValueObject
    {

        #region Fields

        private readonly DbBoundedContext<TContext> context;
        private readonly IObjectTranslator<TStateEntity, TDomainEntity> entityTranslator;
        private readonly IObjectTranslator<IEvent, Event> eventTranslator;
        private readonly IObjectTranslator<Exception, RepositoryException> exceptionTranslator = new EFRepositoryExceptionTranslator();

        #endregion Fields

        #region Constructors

        protected EFRepository(DbBoundedContext<TContext> context,
                               IObjectTranslator<TStateEntity, TDomainEntity> entityTranslator,
                               IObjectTranslator<IEvent, Event> eventTranslator)
        {
            Condition.Requires(context, nameof(context)).IsNotNull();
            Condition.Requires(entityTranslator, nameof(entityTranslator)).IsNotNull();
            Condition.Requires(eventTranslator, nameof(eventTranslator)).IsNotNull();
            this.context = context;
            this.entityTranslator = entityTranslator;
            this.eventTranslator = eventTranslator;
        }

        #endregion Constructors

        #region Properties

        protected DbConnection Connection() => this.context.Database.GetDbConnection();

        #endregion Properties

        #region Methods

        public TDomainEntity Find(TIdentity identity)
        {
            Condition.Requires(identity, nameof(identity)).IsNotNull();
            try
            {
                var keyValues = identity.PrimitiveEqualityComponents();
                // To avoid a promotion to an MSDTC transaction
                this.Connection().OpenIfClosed();
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
            Condition.Requires(identity, nameof(identity)).IsNotNull();
            try
            {
                await new SynchronizationContextRemover();
                var keyValues = identity.PrimitiveEqualityComponents();
                // To avoid a promotion to an MSDTC transaction
                await this.Connection().OpenIfClosedAsync(cancellationToken);
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
            Condition.Requires(aggregate, nameof(aggregate)).IsNotNull();
            try
            {
                var stateEntity = aggregate.ToState();
                var events = ToEvents(aggregate);
                // To avoid a promotion to an MSDTC transaction
                this.Connection().OpenIfClosed();
                this.Save(stateEntity, events);
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<RepositoryException>())
            {
                throw this.TranslateException(ex);
            }
        }

        public async Task SaveAsync(TDomainEntity aggregate, CancellationToken cancellationToken = default)
        {
            Condition.Requires(aggregate, nameof(aggregate)).IsNotNull();
            try
            {
                await new SynchronizationContextRemover();
                var stateEntity = aggregate.ToState();
                var events = ToEvents(aggregate);
                // To avoid a promotion to an MSDTC transaction
                await this.Connection().OpenIfClosedAsync(cancellationToken);
                await this.SaveAsync(stateEntity, events, cancellationToken);
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<RepositoryException>())
            {
                throw this.TranslateException(ex);
            }
        }

        protected virtual TStateEntity Find(IEnumerable<object> keyValues)
        {
            var keyNames = this.context.GetKeyNames<TStateEntity>();
            if (keyValues.Count() != keyNames.Count())
                throw new InvalidOperationException($"You must specify {keyNames.Count()} identity components.");
            var expression = BuildFindExpression(keyNames, keyValues);
            return this.Query().FirstOrDefault(expression);
        }

        protected virtual Task<TStateEntity> FindAsync(IEnumerable<object> keyValues, CancellationToken cancellationToken)
        {
            var keyNames = this.context.GetKeyNames<TStateEntity>();
            if (keyValues.Count() != keyNames.Count())
                throw new InvalidOperationException($"You must specify {keyNames.Count()} identity components.");
            var expression = BuildFindExpression(keyNames, keyValues);
            return this.Query().FirstOrDefaultAsync(expression, cancellationToken);
        }

        protected IQueryable<TStateEntity> Query()
        {
            var query = this.context.Set<TStateEntity>().AsNoTracking().AsQueryable();
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
            this.context.Set<TStateEntity>().Add(stateEntity);
            this.context.Events.AddRange(events);
            this.context.SaveChanges();
        }

        protected virtual Task SaveAsync(TStateEntity stateEntity, IEnumerable<Event> events, CancellationToken cancellationToken)
        {
            this.context.Set<TStateEntity>().Add(stateEntity);
            this.context.Events.AddRange(events);
            return this.context.SaveChangesAsync(cancellationToken);
        }

        protected RepositoryException TranslateException(Exception ex)
        {
            return this.exceptionTranslator.Translate(ex, new { EntityType = typeof(TDomainEntity) });
        }

        protected TDomainEntity TranslateEntity(TStateEntity stateEntity)
        {
            if (stateEntity == null) return null;
            return this.entityTranslator.Translate(stateEntity);
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

        private IEnumerable<Event> ToEvents(TDomainEntity aggregate)
        {
            var guidGenerator = this.Connection().SequentialGuidGenerator();
            var username = Thread.CurrentPrincipal?.Identity?.Name;
            return aggregate.AllEvents().Select(e =>
            {
                var context = new
                {
                    EventId = guidGenerator.Generate(),
                    StreamId = aggregate.IdentityAsString(),
                    StreamType = aggregate.GetType().Name,
                    StreamSource = this.context.Code,
                    IssuedBy = username
                };
                return this.eventTranslator.Translate(e, context);
            });
        }

        #endregion Methods

    }
}