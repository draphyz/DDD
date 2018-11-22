using System;
using System.Collections.Generic;
using Conditions;

namespace DDD.Collections
{
    /// <summary>
    /// Supports the comparison of objects for equality based on a key defined by a lambda expression.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <seealso cref="IEqualityComparer{TSource}" />
    public class KeyEqualityComparer<TSource, TKey> : IEqualityComparer<TSource>
    {
        #region Fields

        private readonly Func<TSource, TKey> keySelector;

        #endregion Fields

        #region Constructors

        public KeyEqualityComparer(Func<TSource, TKey> keySelector)
        {
            Condition.Requires(keySelector, nameof(keySelector)).IsNotNull();
            this.keySelector = keySelector;
        }

        #endregion Constructors

        #region Methods

        public bool Equals(TSource x, TSource y)
        {
            if (x == null || y == null) return (x == null && y == null);
            return object.Equals(keySelector(x), keySelector(y));
        }

        public int GetHashCode(TSource obj)
        {
            if (obj == null) return int.MinValue;
            var k = keySelector(obj);
            if (k == null) return int.MaxValue;
            return k.GetHashCode();
        }

        #endregion Methods
    }
}
