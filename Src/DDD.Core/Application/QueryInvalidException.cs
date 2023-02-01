using System;
using System.Linq;
using System.Runtime.Serialization;

namespace DDD.Core.Application
{
    using Validation;

    /// <summary>
    /// Exception thrown when a query is invalid.
    /// </summary>
    public class QueryInvalidException : QueryException
    {

        #region Constructors

        public QueryInvalidException(IQuery query = null, ValidationFailure[] failures = null, Exception innerException = null)
            : base(false, DefaultMessage(query), query, innerException)
        {
            this.Failures = failures;
        }

        public QueryInvalidException(string message, IQuery query = null, ValidationFailure[] failures = null, Exception innerException = null)
            : base(false, message, query, innerException)
        {
            this.Failures = failures;
        }

        #endregion Constructors

        #region Properties

        public ValidationFailure[] Failures { get; }

        #endregion Properties

        #region Methods

        public static new string DefaultMessage(IQuery query = null)
        {
            if (query == null)
                return "The query is invalid.";
            return $"The query '{query.GetType().Name}' is invalid.";
        }

        public bool HasFailures() => this.Failures != null && this.Failures.Any();

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            if (this.Failures != null)
            {
                for (var i = 0; i < this.Failures.Length; i++)
                    info.AddValue($"Failure{i}", this.Failures[i]);
            }
        }

        public override string ToString()
        {
            var s = $"{this.GetType()}: {this.Message} ";
            s += $"{Environment.NewLine}Timestamp: {this.Timestamp}";
            s += $"{Environment.NewLine}IsTransient: {this.IsTransient}";
            if (this.Query != null)
                s += $"{Environment.NewLine}Query: {this.Query}";
            if (this.Failures != null)
            {
                for (var i = 0; i < this.Failures.Length; i++)
                    s += $"{Environment.NewLine}Failure{i}: {this.Failures[i]}";
            }
            if (this.InnerException != null)
                s += $" ---> {this.InnerException}";
            if (this.StackTrace != null)
                s += $"{Environment.NewLine}{this.StackTrace}";
            return s;
        }

        #endregion Methods

    }
}
