using System;
using System.Threading;
using Conditions;

namespace DDD
{
    public static class ExceptionExtensions
    {

        #region Methods

        public static string FullSource(this Exception exception)
        {
            Condition.Requires(exception, nameof(exception)).IsNotNull();
            if (exception.TargetSite == null) return exception.Source;
            var sourceType = exception.TargetSite.ReflectedType;
            return $"{sourceType.Assembly.GetName().Name}, {sourceType.FullName}, {exception.TargetSite}";
        }

        /// <summary>
        /// Indicates whether this exception represents a contract error : some preconditions, postconditions or object invariants are not met.
        /// </summary>
        public static bool IsContractError(this Exception exception)
        {
            Condition.Requires(exception, nameof(exception)).IsNotNull();
            switch(exception)
            {
                case ArgumentException _:
                case InvalidOperationException _:
                case FormatException _:
                case NotSupportedException _:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Indicates whether this exception should be wrapped.
        /// </summary>
        /// <typeparam name="TDestination">The type of the wrapped exception.</typeparam>
        public static bool ShouldBeWrappedIn<TDestination>(this Exception exception) where TDestination : Exception
        {
            Condition.Requires(exception, nameof(exception)).IsNotNull();
            switch (exception)
            {
                case TDestination _:
                case OutOfMemoryException _:
                case OperationCanceledException _:
                case StackOverflowException _:
                case ThreadAbortException _:
                    return false;
                default:
                    return !exception.IsContractError();
            }
        }

        #endregion Methods

    }
}
