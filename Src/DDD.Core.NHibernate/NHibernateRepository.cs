using NHibernate;
using System.Threading.Tasks;
using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    using Domain;
    using Threading;

    public class NHibernateRepository<TDomainEntity> : IAsyncRepository<TDomainEntity>
        where TDomainEntity : DomainEntity
    {
        #region Fields

        private readonly ISession session;

        #endregion Fields

        #region Constructors

        public NHibernateRepository(ISession session)
        {
            Condition.Requires(session, nameof(session)).IsNotNull();
            this.session = session;
        }

        #endregion Constructors

        #region Methods

        public async Task<TDomainEntity> FindAsync(ComparableValueObject identity)
        {
            Condition.Requires(identity, nameof(identity)).IsNotNull();
            await new SynchronizationContextRemover();
            return await this.session.GetAsync<TDomainEntity>(identity);
        }

        public Task SaveAsync(TDomainEntity aggregate)
        {
            throw new System.NotImplementedException();
        }

        #endregion Methods

    }
}
