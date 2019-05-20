using Conditions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Infrastructure.Data
{
    using Domain;
    using Mapping;
    using Threading;

    public abstract class EFRepository<TDomainEntity, TStateEntity, TContext>
        : IAsyncRepository<TDomainEntity>
        where TDomainEntity : DomainEntity, IStateObjectConvertible<TStateEntity>
        where TStateEntity : class, IStateEntity, new()
        where TContext : StateEntitiesContext
    {

        #region Fields

        private readonly IAsyncDbContextFactory<TContext> contextFactory;

        private readonly IObjectTranslator<TStateEntity, TDomainEntity> entityTranslator;

        private readonly IObjectTranslator<IEvent, EventState> eventTranslator;

        #endregion Fields

        #region Constructors

        protected EFRepository(IObjectTranslator<TStateEntity, TDomainEntity> entityTranslator,
                               IObjectTranslator<IEvent, EventState> eventTranslator,
                               IAsyncDbContextFactory<TContext> contextFactory)
        {
            Condition.Requires(entityTranslator, nameof(entityTranslator)).IsNotNull();
            Condition.Requires(eventTranslator, nameof(eventTranslator)).IsNotNull();
            Condition.Requires(contextFactory, nameof(contextFactory)).IsNotNull();
            this.entityTranslator = entityTranslator;
            this.eventTranslator = eventTranslator;
            this.contextFactory = contextFactory;
        }

        #endregion Constructors

        #region Methods

        public async Task<TDomainEntity> FindAsync(params ComparableValueObject[] identityComponents)
        {
            Condition.Requires(identityComponents, nameof(identityComponents))
                     .IsNotNull()
                     .IsNotEmpty()
                     .DoesNotContain(null);
            await new SynchronizationContextRemover();
            var keyValues = identityComponents.Select(c => c.EqualityComponents().First());
            var stateEntity = await this.FindAsync(keyValues);
            if (stateEntity == null) return null;
            return this.entityTranslator.Translate(stateEntity);
        }

        public async Task SaveAsync(TDomainEntity aggregate)
        {
            Condition.Requires(aggregate, nameof(aggregate)).IsNotNull();
            await new SynchronizationContextRemover();
            var stateEntity = aggregate.ToState();
            var events = ToEventStates(aggregate);
            using (var context = await this.CreateContextAsync())
            {
                context.Set<TStateEntity>().Add(stateEntity);
                context.Set<EventState>().AddRange(events);
                await SaveChangesAsync(context);
            }
        }

        protected virtual async Task<TStateEntity> FindAsync(IEnumerable<object> keyValues)
        {
            using (var context = await this.CreateContextAsync())
            {
                var keyNames = context.GetKeyNames<TStateEntity>();
                if (keyValues.Count() != keyNames.Count())
                    throw new InvalidOperationException($"You must specify {keyNames.Count()} identity components.");
                var query = context.Set<TStateEntity>().AsQueryable();
                foreach (var path in this.RelatedEntitiesPaths())
                    query = query.Include(path);
                var expression = BuildFindExpression(keyNames, keyValues);
                return await query.FirstOrDefaultAsync(expression);
            }
        }

        protected abstract IEnumerable<Expression<Func<TStateEntity, object>>> RelatedEntitiesPaths();

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

        private static async Task SaveChangesAsync(TContext context)
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new RepositoryConcurrencyException(ex, typeof(TDomainEntity));
            }
            catch (Exception ex) when (ex is DbUpdateException || ex is DbEntityValidationException)
            {
                throw new RepositoryException(ex, typeof(TDomainEntity));
            }
        }

        private async Task<TContext> CreateContextAsync()
        {
            try
            {
                return await this.contextFactory.CreateContextAsync();
            }
            catch (DbException ex)
            {
                throw new RepositoryException(ex, typeof(TDomainEntity));
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