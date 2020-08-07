using System;
using System.Collections.Generic;
using Conditions;

namespace DDD.Collections
{
    /// <summary>
    /// Adds extension methods to the <see cref="IEnumerable{T}" /> interface.
    /// </summary>
    public static class IEnumerableExtensions
    {

        #region Methods

        public static int CombineHashCodes(this IEnumerable<object> collection)
        {
            Condition.Requires(collection, nameof(collection)).IsNotNull();
            unchecked
            {
                var hash = 17;
                foreach (var obj in collection)
                    hash = hash * 23 + (obj != null ? obj.GetHashCode() : 0);
                return hash;
            }
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            Condition.Requires(collection, nameof(collection)).IsNotNull();
            Condition.Requires(action, nameof(action)).IsNotNull();
            foreach (var item in collection)
                action(item);
            return collection;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<int, T> action)
        {
            Condition.Requires(collection, nameof(collection)).IsNotNull();
            Condition.Requires(action, nameof(action)).IsNotNull();
            var i = 0;
            foreach (var item in collection)
            {
                action(i, item);
                i++;
            }
            return collection;
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> collection)
        {
            Condition.Requires(collection, nameof(collection)).IsNotNull();
            return new HashSet<T>(collection);
        }

        #endregion Methods

    }
}