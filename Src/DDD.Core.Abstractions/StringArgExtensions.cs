using EnsureThat;
using EnsureThat.Enforcers;
using System;

namespace DDD
{
    public static class StringArgExtensions
    {

        #region Methods

        /// <summary>
        /// Ensures that the string has a minimum length.
        /// </summary>
        public static string HasMinLength(this StringArg _, string value, int minLength, string paramName = null, OptsFn optsFn = null)
        {
            Ensure.Any.IsNotNull(value, paramName, optsFn);

            if (value.Length < minLength)
                throw Ensure.ExceptionFactory.ArgumentException(
                    string.Format("The string is not long enough. Must be longer than '{0}' but was '{1}' characters long.", minLength, value.Length),
                    paramName,
                    optsFn);

            return value;
        }

        /// <summary>
        /// Ensures that the string contains only digits.
        /// </summary>
        public static string IsAllDigits(this StringArg _, string value, string paramName = null, OptsFn optsFn = null)
        {
            Ensure.Any.IsNotNull(value, paramName, optsFn);

            for (var i = 0; i < value.Length; i++)
                if (!char.IsDigit(value[i]))
                    throw Ensure.ExceptionFactory.ArgumentException(
                        string.Format("Expected '{0} to contain only digits but does not.", value),
                        paramName,
                        optsFn);

            return value;
        }

        /// <summary>
        /// Ensures that the string contains only letters.
        /// </summary>
        public static string IsAllLetters(this StringArg _, string value, string paramName = null, OptsFn optsFn = null)
        {
            Ensure.Any.IsNotNull(value, paramName, optsFn);

            for (var i = 0; i < value.Length; i++)
                if (!char.IsLetter(value[i]))
                    throw Ensure.ExceptionFactory.ArgumentException(
                        string.Format("Expected '{0} to contain only letters but does not.", value),
                        paramName,
                        optsFn);

            return value;
        }

        /// <summary>
        /// Ensures that the string satisfies the specified condition.
        /// </summary>
        public static string Satisfy(this StringArg _, string value, Func<string, bool> predicate, string paramName = null, OptsFn optsFn = null)
        {
            Ensure.Any.Satisfy(value, predicate, paramName, optsFn);
            return value;
        }

        #endregion Methods

    }
}
