namespace DDD.Core.Domain
{
    public interface IRepository<TDomainEntity, in TIdentity>
        where TDomainEntity : DomainEntity
        where TIdentity : ComparableValueObject
    {
        #region Methods

        TDomainEntity Find(TIdentity identity);

        void Save(TDomainEntity aggregate);

        #endregion Methods
    }
}
