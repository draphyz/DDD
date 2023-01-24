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

        void Save(TDomainEntity aggregate);

        #endregion Methods
    }
}
