using Conditions;
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Domain;
    using Serialization;


    public abstract class EFRepositoryWithEventStoring<TDomainEntity, TStateEntity, TContext>
        : EFRepository<TDomainEntity, TStateEntity, TContext>
        where TDomainEntity : DomainEntity, IStateObjectConvertible<TStateEntity>
        where TStateEntity : class, IStateEntity, new()
        where TContext : StateEntitiesContext
    {

        #region Constructors

        protected EFRepositoryWithEventStoring(IObjectTranslator<TStateEntity, TDomainEntity> entityTranslator,
                                               IObjectTranslator<IDomainEvent, StoredEvent> eventTranslator,
                                               IDbContextFactory<TContext> contextFactory)
            : base(entityTranslator, contextFactory)
        {
            Condition.Requires(eventTranslator, nameof(eventTranslator)).IsNotNull();
            this.EventTranslator = eventTranslator;
        }

        #endregion Constructors

        #region Properties

        protected IObjectTranslator<IDomainEvent, StoredEvent> EventTranslator { get; }

        #endregion Properties

        #region Methods

        public override async Task SaveAsync(TDomainEntity aggregate)
        {
            Condition.Requires(aggregate, nameof(aggregate)).IsNotNull();
            var events = ToStoredEvents(new TDomainEntity[] { aggregate });
            using (var context = this.CreateContext())
            {
                context.Set<TStateEntity>().Add(aggregate.ToState());
                context.Set<StoredEvent>().AddRange(events);
                await SaveChangesAsync(context);
            }
        }

        public override async Task SaveAllAsync(IEnumerable<TDomainEntity> aggregates)
        {
            Condition.Requires(aggregates, nameof(aggregates))
                     .IsNotNull()
                     .IsNotEmpty()
                     .DoesNotContain(null);
            var events = ToStoredEvents(aggregates);
            using (var context = this.CreateContext())
            {
                context.Set<TStateEntity>().AddRange(aggregates.Select(a => a.ToState()));
                context.Set<StoredEvent>().AddRange(events);
                await SaveChangesAsync(context);
            }
        }

        private IEnumerable<StoredEvent> ToStoredEvents(IEnumerable<TDomainEntity> aggregates)
        {
            var commitId = Guid.NewGuid();
            var subject = Thread.CurrentPrincipal?.Identity?.Name;
            return aggregates.SelectMany(a => a.AllEvents().Select(e =>
            {
                var evt = this.EventTranslator.Translate(e);
                evt.StreamId = a.IdentityAsString();
                evt.CommitId = commitId;
                evt.Subject = subject;
                return evt;
            }));
        }

        #endregion Methods

    }
}