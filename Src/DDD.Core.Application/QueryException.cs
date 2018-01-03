using System;

namespace DDD.Core.Application
{
    /// <summary>
    /// Exception thrown when a query failed.
    /// </summary>
    public class QueryException : Exception
    {

        #region Constructors

        public QueryException()
            : base("The query failed.")
        {
        }

        public QueryException(string message) : base(message)
        {
        }

        public QueryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public QueryException(string message, Exception innerException, IQuery query) : base(message, innerException)
        {
            this.Query = query;
        }

        #endregion Constructors

        #region Properties

        public IQuery Query { get; }

        #endregion Properties

    }
}
