using System.Threading.Tasks;
using Conditions;

namespace DDD.Core.Domain
{
    using Threading;

    public static class IAsyncRepositoryExtensions
    {

        #region Methods

        public static async Task SaveAsync<TDomainEntity>(this IAsyncRepository<TDomainEntity> repository, 
                                                          params TDomainEntity[] aggregates)
            where TDomainEntity : DomainEntity
        {
            Condition.Requires(repository, nameof(repository)).IsNotNull();
            await new SynchronizationContextRemover();
            await repository.SaveAsync(aggregates);
        }

        #endregion Methods

    }
}
