using EnsureThat;
using EnsureThat.Enforcers;
using System.Collections.Generic;
using System.Linq;

namespace DDD
{
    public static class EnumerableArgExtensions
    {

        /// <summary>
        /// Ensures that the sequence does not contain null values.
        /// </summary>
        public static IEnumerable<T> HasNoNull<T>(this EnumerableArg _, IEnumerable<T> value, string paramName = null, OptsFn optsFn = null)
        {
            Ensure.Any.IsNotNull(value, paramName);

            if (value.Any(i => i == null))
                throw Ensure.ExceptionFactory.ArgumentException(
                    "Sequence contains null values.",
                    paramName,
                    optsFn);

            return value;
        }

    }
}
