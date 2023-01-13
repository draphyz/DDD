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

        /// <remarks>
        /// Do not respect the DDD rule 'One aggregate modified by transaction' (for simplicity).
        /// </remarks>
        Task SaveAsync(IEnumerable<TDomainEntity> aggregates, CancellationToken cancellationToken = default);

        #endregion Methods
    }
}
