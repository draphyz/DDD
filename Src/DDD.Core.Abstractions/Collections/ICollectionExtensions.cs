using System.Collections.Generic;
using Conditions;

namespace DDD.Collections
{
    /// <summary>
    /// Adds extension methods to the <see cref="ICollection{T}" /> interface.
    /// </summary>
    public static class ICollectionExtensions
    {

        #region Methods

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            Condition.Requires(collection, nameof(collection)).IsNotNull();
            Condition.Requires(items, nameof(items)).IsNotNull();
            foreach(var item in items)
                collection.Add(item);
        }

        #endregion Methods

    }
}
