using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Domain
{
    /// <summary>
    /// Defines a component to find and save asynchronously an aggregate.
    /// </summary>
    public interface IAsyncRepository<TDomainEntity, in TIdentity>
        where TDomainEntity : DomainEntity
        where TIdentity : ComparableValueObject
    {
        #region Methods

        Task<TDomainEntity> FindAsync(TIdentity identity, CancellationToken cancellationToken = default);

        Task SaveAsync(TDomainEntity aggregate, CancellationToken cancellationToken = default);

        #endregion Methods
    }
}