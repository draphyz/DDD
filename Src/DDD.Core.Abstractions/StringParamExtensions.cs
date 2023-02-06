using System;
using EnsureThat;

namespace DDD
{
    public static class StringParamExtensions
    {

        #region Methods

        /// <summary>
        /// Ensures that the string has a minimum length.
        /// </summary>
        public static void HasMinLength(this in StringParam param, int minLength)
            => Ensure.String.HasMinLength(param.Value, minLength, param.Name, param.OptsFn);

        /// <summary>
        /// Ensures that the string contains only digits.
        /// </summary>
        public static void IsAllDigits(this in StringParam param)
            => Ensure.String.IsAllDigits(param.Value, param.Name, param.OptsFn);

        /// <summary>
        /// Ensures that the string contains only letters.
        /// </summary>
        public static void IsAllLetters(this in StringParam param)
            => Ensure.String.IsAllLetters(param.Value, param.Name, param.OptsFn);

        /// <summary>
        /// Ensures that the string satisfies the specified condition.
        /// </summary>
        public static void Satisfy(this in StringParam param, Func<string, bool> predicate)
            => Ensure.String.Satisfy(param.Value, predicate, param.Name, param.OptsFn);

        #endregion Methods

    }
}