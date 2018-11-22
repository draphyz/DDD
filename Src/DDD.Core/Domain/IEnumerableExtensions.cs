using System.Collections.Generic;
using System.Linq;

namespace DDD.Core.Domain
{
    public static class IEnumerableExtensions
    {

        #region Methods

        public static IEnumerable<IDomainEvent> AllEvents(this IEnumerable<DomainEntity> entities)
        {
            return entities.SelectMany(e => e.AllEvents()).OrderBy(e => e.OccurredOn);
        }

        #endregion Methods

    }
}
