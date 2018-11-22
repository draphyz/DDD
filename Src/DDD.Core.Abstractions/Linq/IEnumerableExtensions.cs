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

        #endregion Methods
    }
}