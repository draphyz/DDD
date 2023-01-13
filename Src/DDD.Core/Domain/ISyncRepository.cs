using System.Collections.Generic;

namespace DDD.Core.Domain
{
    /// <summary>
    /// Defines a component to find and save synchronously an aggregate.
    /// </summary>
    public interface ISyncRepository<TDomainEntity, in TIdentity>
        where TDomainEntity : DomainEntity
        where TIdentity : ComparableValueObject
    {
        #region Methods

        TDomainEntity Find(TIdentity identity);

        /// <remarks>
        /// Do not respect the DDD rule 'One aggregate modified by transaction' (for simplicity).
        /// </remarks>
        void Save(IEnumerable<TDomainEntity> aggregates);

        #endregion Methods
    }
}
