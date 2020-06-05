using Conditions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
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

    public abstract class EFRepository<TDomainEntity, TStateEntity>
        : IAsyncRepository<TDomainEntity>
        where TDomainEntity : DomainEntity, IStateObjectConvertible<TStateEntity>
        where TStateEntity : class, IStateEntity, new()
    {

        #region Fields

        private readonly StateEntitiesContext context;
        private readonly IObjectTranslator<TStateEntity, TDomainEntity> entityTranslator;
        private readonly IObjectTranslator<IEvent, EventState> eventTranslator;
        private readonly IObjectTranslator<Exception, RepositoryException> exceptionTranslator = EFRepositoryExceptionTranslator.Default;

        #endregion Fields

        #region Constructors

        protected EFRepository(StateEntitiesContext context,
                               IObjectTranslator<TStateEntity, TDomainEntity> entityTranslator,
                               IObjectTranslator<IEvent, EventState> eventTranslator)
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

        protected DbConnection Connection => this.context.Database.Connection;

        #endregion Properties

        #region Methods

        public async Task<TDomainEntity> FindAsync(ComparableValueObject identity)
        {
            Condition.Requires(identity, nameof(identity)).IsNotNull();
            await new SynchronizationContextRemover();
            var keyValues = identity.PrimitiveEqualityComponents();
            await this.OpenConnectionAsync();
            var stateEntity = await this.FindAsync(keyValues);
            return this.TranslateEntity(stateEntity);
        }

        public async Task SaveAsync(TDomainEntity aggregate)
        {
            Condition.Requires(aggregate, nameof(aggregate)).IsNotNull();
            await new SynchronizationContextRemover();
            var stateEntity = aggregate.ToState();
            var events = ToEventStates(aggregate);
            await this.OpenConnectionAsync();
            await this.SaveAsync(stateEntity, events);
        }

        protected virtual async Task<TStateEntity> FindAsync(IEnumerable<object> keyValues)
        {
            var keyNames = this.context.GetKeyNames<TStateEntity>();
            if (keyValues.Count() != keyNames.Count())
                throw new InvalidOperationException($"You must specify {keyNames.Count()} identity components.");
            var expression = BuildFindExpression(keyNames, keyValues);
            return await this.Query().FirstOrDefaultAsync(expression);
        }

        /// <remarks>To avoid a promotion to an MSDTC transaction</remarks>
        protected async Task OpenConnectionAsync()
        {
            try
            {
                if (this.Connection.State == ConnectionState.Closed)
                    await this.Connection.OpenAsync();
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

        protected virtual async Task SaveAsync(TStateEntity stateEntity, IEnumerable<EventState> events)
        {
            this.context.Set<TStateEntity>().Add(stateEntity);
            this.context.Set<EventState>().AddRange(events);
            await this.SaveChangesAsync();
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

        private async Task SaveChangesAsync()
        {
            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (Exception ex) when (ex is DbUpdateException || ex is DbEntityValidationException)
            {
                throw this.exceptionTranslator.Translate(ex, new { EntityType = typeof(TDomainEntity) });
            }
        }

        private IEnumerable<EventState> ToEventStates(TDomainEntity aggregate)
        {
            var commitId = Guid.NewGuid();
            var subject = Thread.CurrentPrincipal?.Identity?.Name;
            return aggregate.AllEvents().Select(e =>
            {
                var evt = this.eventTranslator.Translate(e);
                evt.StreamId = aggregate.IdentityAsString();
                evt.CommitId = commitId;
                evt.Subject = subject;
                return evt;
            });
        }

        #endregion Methods

    }
}