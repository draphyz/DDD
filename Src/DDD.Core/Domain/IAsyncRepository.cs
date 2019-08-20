using System.Threading.Tasks;

namespace DDD.Core.Domain
{
    public interface IAsyncRepository<TDomainEntity, in TIdentity>
        where TDomainEntity : DomainEntity
        where TIdentity : ComparableValueObject
    {
        #region Methods

        Task<TDomainEntity> FindAsync(TIdentity identity);

        Task SaveAsync(TDomainEntity aggregate);

        #endregion Methods
    }
}
