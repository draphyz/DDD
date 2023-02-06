using EnsureThat;
using EnsureThat.Enforcers;
using System;

namespace DDD
{
    public static class AnyArgExtensions
    {

        /// <summary>
        /// Ensures that the argument satisfies the specified condition.
        /// </summary>
        public static T Satisfy<T>(this AnyArg _, T value, Func<T, bool> predicate, string paramName = null, OptsFn optsFn = null)
        {
            if (predicate != null && !predicate(value))
                throw Ensure.ExceptionFactory.ArgumentException("The argument does not satisfy the specified condition.", paramName, optsFn);

            return value;
        }

    }
}
