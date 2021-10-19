using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Domain
{
    public interface IAsyncRepository<TDomainEntity>
        where TDomainEntity : DomainEntity
    {
        #region Methods

        Task<TDomainEntity> FindAsync(ComparableValueObject identity, CancellationToken cancellationToken = default);

        Task SaveAsync(TDomainEntity aggregate, CancellationToken cancellationToken = default);

        #endregion Methods
    }
}
