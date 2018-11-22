using System;

namespace DDD.Core.Application
{
    /// <summary>
    /// Exception thrown when a query failed.
    /// </summary>
    [Serializable]
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

        public QueryException(Exception innerException, IQuery query) 
            : base($"The query '{query.GetType().Name}' failed.", innerException)
        {
            this.Query = query;
        }

        #endregion Constructors

        #region Properties

        public IQuery Query { get; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            var s = $"{this.GetType()}: {this.Message} ";
            if (this.Query != null)
                s += $"{Environment.NewLine}Query: {this.Query}";
            if (this.InnerException != null)
                s += $" ---> {this.InnerException}";
            if (this.StackTrace != null)
                s += $"{Environment.NewLine}{this.StackTrace}";
            return s;
        }

        #endregion Methods
    }
}
