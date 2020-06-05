using System;

namespace DDD.Core.Application
{
    /// <summary>
    /// Exception thrown when a query was denied.
    /// </summary>
    public class QueryUnauthorizedException : QueryException
    {

        #region Constructors

        public QueryUnauthorizedException(IQuery query = null, Exception innerException = null)
            : base(false, DefaultMessage(query), query, innerException)
        {
        }

        public QueryUnauthorizedException(string message, IQuery query = null, Exception innerException = null)
            : base(false, message, query, innerException)
        {
        }

        #endregion Constructors

        #region Methods

        public static new string DefaultMessage(IQuery query = null)
        {
            if (query == null)
                return "The query was denied.";
            return $"The query '{query.GetType().Name}' was denied.";
        }

        #endregion Methods

    }
}
