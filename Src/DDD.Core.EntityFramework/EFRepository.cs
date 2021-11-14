using Conditions;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace DDD.Core.Infrastructure.Data
{
    using Domain;
    using Mapping;
    using Threading;

    public abstract class EFRepository<TDomainEntity, TStateEntity, TIdentity>
        : IAsyncRepository<TDomainEntity, TIdentity>
        where TDomainEntity : DomainEntity, IStateObjectConvertible<TStateEntity>
        where TStateEntity : class, IStateEntity, new()
        where TIdentity : ComparableValueObject
    {

        #region Fields

        private readonly BoundedContext context;
        private readonly IObjectTranslator<TStateEntity, TDomainEntity> entityTranslator;
        private readonly IObjectTranslator<IEvent, StoredEvent> eventTranslator;
        private readonly IObjectTranslator<Exception, RepositoryException> exceptionTranslator = EFRepositoryExceptionTranslator.Default;

        #endregion Fields

        #region Constructors

        protected EFRepository(BoundedContext context,
                               IObjectTranslator<TStateEntity, TDomainEntity> entityTranslator,
                               IObjectTranslator<IEvent, StoredEvent> eventTranslator)
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

        public async Task<TDomainEntity> FindAsync(TIdentity identity, CancellationToken cancellationToken = default)
        {
            Condition.Requires(identity, nameof(identity)).IsNotNull();
            await new SynchronizationContextRemover();
            var keyValues = identity.PrimitiveEqualityComponents();
            await this.OpenConnectionAsync(cancellationToken);
            var stateEntity = await this.FindAsync(keyValues, cancellationToken);
            return this.TranslateEntity(stateEntity);
        }

        public async Task SaveAsync(TDomainEntity aggregate, CancellationToken cancellationToken = default)
        {
            Condition.Requires(aggregate, nameof(aggregate)).IsNotNull();
            await new SynchronizationContextRemover();
            var stateEntity = aggregate.ToState();
            var events = ToStoredEvents(aggregate);
            await this.OpenConnectionAsync(cancellationToken);
            await this.SaveAsync(stateEntity, events, cancellationToken);
        }

        protected virtual async Task<TStateEntity> FindAsync(IEnumerable<object> keyValues, CancellationToken cancellationToken = default)
        {
            var keyNames = this.context.GetKeyNames<TStateEntity>();
            if (keyValues.Count() != keyNames.Count())
                throw new InvalidOperationException($"You must specify {keyNames.Count()} identity components.");
            var expression = BuildFindExpression(keyNames, keyValues);
            return await this.Query().FirstOrDefaultAsync(expression, cancellationToken);
        }

        /// <remarks>To avoid a transaction promotion from local to distributed</remarks>
        protected async Task OpenConnectionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var connection = this.Connection();
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync(cancellationToken);
            }
            catch (DbException ex)
            {
                throw this.exceptionTranslator.Translate(ex, new { EntityType = typeof(TDomainEntity) });
            }
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

        protected virtual async Task SaveAsync(TStateEntity stateEntity, IEnumerable<StoredEvent> events, CancellationToken cancellationToken = default)
        {
            this.context.Set<TStateEntity>().Add(stateEntity);
            this.context.Set<StoredEvent>().AddRange(events);
            await this.SaveChangesAsync(cancellationToken);
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

        private async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                await this.context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                throw this.exceptionTranslator.Translate(ex, new { EntityType = typeof(TDomainEntity) });
            }
        }

        private IEnumerable<StoredEvent> ToStoredEvents(TDomainEntity aggregate)
        {
            var username = Thread.CurrentPrincipal?.Identity?.Name;
            return aggregate.AllEvents().Select(e =>
            {
                var evt = this.eventTranslator.Translate(e);
                evt.StreamId = aggregate.IdentityAsString();
                evt.StreamType = aggregate.GetType().Name;
                evt.IssuedBy = username;
                return evt;
            });
        }

        #endregion Methods

    }
}