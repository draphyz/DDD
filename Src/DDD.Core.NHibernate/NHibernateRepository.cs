using Conditions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Infrastructure.Data
{
    using Domain;
    using Mapping;
    using Threading;

    public class NHibernateRepository<TDomainEntity, TIdentity> : IAsyncRepository<TDomainEntity, TIdentity>
        where TDomainEntity : DomainEntity
        where TIdentity : ComparableValueObject
    {

        #region Fields

        private readonly IObjectTranslator<IEvent, StoredEvent> eventTranslator;
        private readonly IObjectTranslator<Exception, RepositoryException> exceptionTranslator = NHibernateRepositoryExceptionTranslator.Default;
        private readonly ISession session;

        #endregion Fields

        #region Constructors

        public NHibernateRepository(ISession session, IObjectTranslator<IEvent, StoredEvent> eventTranslator)
        {
            Condition.Requires(session, nameof(session)).IsNotNull();
            Condition.Requires(eventTranslator, nameof(eventTranslator)).IsNotNull();
            this.session = session;
            this.eventTranslator = eventTranslator;
        }

        #endregion Constructors

        #region Methods

        public async Task<TDomainEntity> FindAsync(TIdentity identity, CancellationToken cancellationToken = default)
        {
            Condition.Requires(identity, nameof(identity)).IsNotNull();
            await new SynchronizationContextRemover();
            try
            {
                return await this.session.GetAsync<TDomainEntity>(identity, cancellationToken);
            }
            catch (HibernateException ex)
            {
                throw this.exceptionTranslator.Translate(ex, new { EntityType = typeof(TDomainEntity) });
            }
        }

        public async Task SaveAsync(TDomainEntity aggregate, CancellationToken cancellationToken = default)
        {
            var events = ToStoredEvents(aggregate);
            await new SynchronizationContextRemover();
            try
            {
                await this.session.SaveOrUpdateAsync(aggregate, cancellationToken);
                foreach (var @event in events)
                    await this.session.SaveAsync(@event, cancellationToken);
            }
            catch (HibernateException ex)
            {
                throw this.exceptionTranslator.Translate(ex, new { EntityType = typeof(TDomainEntity) });
            }
        }

        private IEnumerable<StoredEvent> ToStoredEvents(TDomainEntity aggregate)
        { 
            var user = Thread.CurrentPrincipal?.Identity?.Name;
            return aggregate.AllEvents().Select(e =>
            {
                var evt = this.eventTranslator.Translate(e);
                evt.StreamType = aggregate.GetType().Name;
                evt.StreamId = aggregate.IdentityAsString();
                evt.UniqueId = Guid.NewGuid();
                evt.Username = user;
                return evt;
            });
        }

        #endregion Methods

    }
}
