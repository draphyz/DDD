using Conditions;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Domain
{
    public static class IRepositoryExtensions
    {

        #region Methods

        public static void Save<TDomainEntity, TIdentity>(this ISyncRepository<TDomainEntity, TIdentity> repository, params TDomainEntity[] aggregates)
            where TDomainEntity : DomainEntity
            where TIdentity : ComparableValueObject
        {
            Condition.Requires(repository, nameof(repository)).IsNotNull();
            repository.Save(aggregates);
        }

        public static Task SaveAsync<TDomainEntity, TIdentity>(this IAsyncRepository<TDomainEntity, TIdentity> repository, CancellationToken cancellationToken, params TDomainEntity[] aggregates)
            where TDomainEntity : DomainEntity
            where TIdentity : ComparableValueObject
        {
            Condition.Requires(repository, nameof(repository)).IsNotNull();
            return repository.SaveAsync(aggregates, cancellationToken);
        }

        #endregion Methods

    }
}