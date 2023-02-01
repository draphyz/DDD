using System;
using System.Linq;
using System.Text;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.Exceptions;

namespace DDD.Core.Infrastructure.Testing
{
    public static class SubstituteExtensions
    {

        #region Methods

        /// <summary>
        /// Checks this substitute has received a matching call the required number of times.
        /// </summary>
        /// <remarks>Quick fix for <see href="https://github.com/nsubstitute/NSubstitute/issues/597">that issue</see></remarks>
        public static T Received<T>(this T substitute, Predicate<ICall> predicate, int requiredNumberOfCalls = 1) where T : class
        {
            if (substitute == null) throw new NullSubstituteReferenceException();
            var numberOfMatchingCalls = substitute.ReceivedCalls().Count(c => predicate(c));
            if (numberOfMatchingCalls != requiredNumberOfCalls)
            {
                var message = new StringBuilder();
                message.AppendLine(string.Format("Expected to receive {0} {1} matching.", requiredNumberOfCalls, requiredNumberOfCalls == 1 ? "call" : "calls"));
                if (numberOfMatchingCalls == 0)
                    message.AppendLine("Actually received no matching calls.");
                else
                    message.AppendLine(string.Format("Actually received {0} matching {1}.", numberOfMatchingCalls, numberOfMatchingCalls == 1 ? "call" : "calls"));
                throw new ReceivedCallsException(message.ToString());
            }
            return substitute;
        }

        #endregion Methods

    }

}
