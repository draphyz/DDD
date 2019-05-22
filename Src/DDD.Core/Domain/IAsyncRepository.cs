using System.Threading.Tasks;

namespace DDD.Core.Domain
{
    public interface IAsyncRepository<TDomainEntity>
        where TDomainEntity : DomainEntity
    {
        #region Methods

        Task<TDomainEntity> FindAsync(ComparableValueObject identity);

        Task SaveAsync(TDomainEntity aggregate);

        #endregion Methods
    }
}
