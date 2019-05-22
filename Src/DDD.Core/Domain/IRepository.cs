namespace DDD.Core.Domain
{
    public interface IRepository<TDomainEntity>
        where TDomainEntity : DomainEntity
    {
        #region Methods

        TDomainEntity Find(ComparableValueObject identity);

        void Save(TDomainEntity aggregate);

        #endregion Methods
    }
}
