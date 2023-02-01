namespace DDD.Core.Domain
{
    public interface IRepository<TDomainEntity, in TIdentity> 
        : ISyncRepository<TDomainEntity, TIdentity>, IAsyncRepository<TDomainEntity, TIdentity>
        where TDomainEntity : DomainEntity
        where TIdentity : ComparableValueObject
    {
    }
}