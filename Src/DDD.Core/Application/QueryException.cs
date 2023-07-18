﻿using System;
using System.Runtime.Serialization;

namespace DDD.Core.Application
{
    /// <summary>
    /// Exception thrown when a query failed.
    /// </summary>
    public class QueryException : ApplicationException
    {

        #region Constructors

        public QueryException(bool isTransient, IQuery query = null, Exception innerException = null)
            : base(isTransient, DefaultMessage(query), innerException)
        {
            this.Query = query;
        }

        public QueryException(bool isTransient, string message, IQuery query = null, Exception innerException = null)
            : base(isTransient, message, innerException)
        {
            this.Query = query;
        }

        #endregion Constructors

        #region Properties

        public IQuery Query { get; }

        #endregion Properties

        #region Methods

        public static string DefaultMessage(IQuery query = null)
        {
            if (query == null)
                return "A query failed.";
            return $"The query '{query.GetType().Name}' failed.";
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            if (this.Query != null)
                info.AddValue("Query", this.Query);
        }

        public override string ToString()
        {
            var s = $"{this.GetType()}: {this.Message} ";
            s += $"{Environment.NewLine}{nameof(Timestamp)}: {this.Timestamp}";
            s += $"{Environment.NewLine}{nameof(IsTransient)}: {this.IsTransient}";
            if (this.Query != null)
                s += $"{Environment.NewLine}{nameof(Query)}: {this.Query}";
            if (this.InnerException != null)
                s += $" ---> {this.InnerException}";
            if (this.StackTrace != null)
                s += $"{Environment.NewLine}{this.StackTrace}";
            return s;
        }

        #endregion Methods
    }
}
