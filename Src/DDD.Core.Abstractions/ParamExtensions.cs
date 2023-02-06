using EnsureThat;
using System;

namespace DDD
{
    public static class ParamExtensions
    {

        #region Methods

        /// <summary>
        /// Ensures that the argument satisfies the specified condition.
        /// </summary>
        public static void Satisfy<T>(this in Param<T> param, Func<T, bool> predicate)
            => Ensure.Any.Satisfy(param.Value, predicate, param.Name, param.OptsFn);

        #endregion Methods

    }
}
