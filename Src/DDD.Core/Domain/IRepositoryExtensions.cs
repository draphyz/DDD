using Conditions;

namespace DDD.Core.Domain
{
    public static class IRepositoryExtensions
    {

        #region Methods

        public static void Save<TDomainEntity>(this IRepository<TDomainEntity> repository, 
                                               params TDomainEntity[] aggregates)
            where TDomainEntity : DomainEntity
        {
            Condition.Requires(repository, nameof(repository)).IsNotNull();
            repository.Save(aggregates);
        }

        #endregion Methods

    }
}
