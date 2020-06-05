using System;

namespace DDD.Core.Application
{
    /// <summary>
    /// Exception thrown when a query cannot be currently handled.
    /// </summary>
    public class QueryUnavailableException : QueryException
    {

        #region Constructors

        public QueryUnavailableException(IQuery query = null, Exception innerException = null)
            : base(true, DefaultMessage(query), query, innerException)
        {
        }

        public QueryUnavailableException(string message, IQuery query = null, Exception innerException = null)
            : base(true, message, query, innerException)
        {
        }

        #endregion Constructors

        #region Methods

        public static new string DefaultMessage(IQuery query = null)
        {
            if (query == null)
                return "The query cannot be currently handled.";
            return $"The query '{query.GetType().Name}' cannot be currently handled.";
        }

        #endregion Methods

    }
}
