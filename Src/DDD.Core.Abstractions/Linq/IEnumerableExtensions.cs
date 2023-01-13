using System;
using System.Linq;
using System.Collections.Generic;
using Conditions;

namespace DDD.Linq
{
    using Collections;

    /// <summary>
    /// Adds extension methods to the <see cref="IEnumerable{T}" /> interface.
    /// </summary>
    public static class IEnumerableExtensions
    {

        #region Methods

        public static IEnumerable<TSource> Except<TSource, TComparable>(this IEnumerable<TSource> source1,
                                                                        IEnumerable<TSource> source2,
                                                                        Func<TSource, TComparable> keySelector)
        {
            Condition.Requires(source1, nameof(source1)).IsNotNull();
            Condition.Requires(source2, nameof(source2)).IsNotNull();
            Condition.Requires(keySelector, nameof(keySelector)).IsNotNull();
            return source1.Except(source2, new KeyEqualityComparer<TSource, TComparable>(keySelector));
        }

        public static IEnumerable<TSource> Union<TSource, TComparable>(this IEnumerable<TSource> source1,
                                                                       IEnumerable<TSource> source2,
                                                                       Func<TSource, TComparable> keySelector)
        {
            Condition.Requires(source1, nameof(source1)).IsNotNull();
            Condition.Requires(source2, nameof(source2)).IsNotNull();
            Condition.Requires(keySelector, nameof(keySelector)).IsNotNull();
            return source1.Union(source2, new KeyEqualityComparer<TSource, TComparable>(keySelector));
        }

        /// <summary>
        /// Returns a new enumerable collection that contains the elements from source with the last count elements of the source collection omitted.
        /// </summary>
        /// <remarks>
        /// If count is not a positive number, this method returns an identical copy of the source enumerable collection.
        /// </remarks>
        public static IEnumerable<TSource> SkipLast<TSource>(this IEnumerable<TSource> source, int count)
        {
            Condition.Requires(source, nameof(source)).IsNotNull();
            if (count <= 0)
            {
                foreach (var item in source)
                    yield return item;
            }
            else
            {
                var queue = new Queue<TSource>();
                using (var e = source.GetEnumerator())
                {
                    while (e.MoveNext())
                    {
                        if (queue.Count == count)
                        {
                            do
                            {
                                yield return queue.Dequeue();
                                queue.Enqueue(e.Current);
                            }
                            while (e.MoveNext());
                        }
                        else
                            queue.Enqueue(e.Current);
                    }
                }
            }
        }

        #endregion Methods
    }
}