using System;

namespace DDD.Core.Application
{
    /// <summary>
    /// Exception thrown when a query has expired.
    /// </summary>
    public class QueryTimeoutException : QueryException
    {

        #region Constructors

        public QueryTimeoutException(IQuery query = null, Exception innerException = null)
            : base(true, DefaultMessage(query), query, innerException)
        {
        }

        public QueryTimeoutException(string message, IQuery query = null, Exception innerException = null)
            : base(true, message, query, innerException)
        {
        }

        #endregion Constructors

        #region Methods

        public static new string DefaultMessage(IQuery query = null)
        {
            if (query == null)
                return "The query has expired.";
            return $"The query '{query.GetType().Name}' has expired.";
        }

        #endregion Methods

    }
}
