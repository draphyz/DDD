using System;

namespace DDD.Core.Application
{
    /// <summary>
    /// Exception thrown when a conflict with other querys has been detected while handling a query.
    /// </summary>
    public class QueryConflictException : QueryException
    {

        #region Constructors

        public QueryConflictException(IQuery query = null, Exception innerException = null)
            : base(true, DefaultMessage(query), query, innerException)
        {
        }

        public QueryConflictException(string message, IQuery query = null, Exception innerException = null)
            : base(true, message, query, innerException)
        {
        }

        #endregion Constructors

        #region Methods

        public static new string DefaultMessage(IQuery query = null)
        {
            if (query == null)
                return "A conflict has been detected while handling a query.";
            return $"A conflict has been detected while handling the query '{query.GetType().Name}'.";
        }

        #endregion Methods

    }
}
