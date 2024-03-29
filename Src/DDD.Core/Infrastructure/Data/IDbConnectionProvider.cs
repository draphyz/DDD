﻿using System;
using System.Data.Common;

namespace DDD.Core.Infrastructure.Data
{
    using Domain;

    /// <summary>
    /// Provides and shares a database connection for a specific bounded context between different components.
    /// This component owns the connection and is responsible for disposing the connection.
    /// </summary>
    /// <remarks>
    /// When you use TransactionScope to define the scope of a business transaction, you must use a single connection to avoid escalating local transactions automatically to distributed transaction managed by the Microsoft DTC.
    /// This connection must be also opened only once until it is disposed.
    /// </remarks>
    public interface IDbConnectionProvider : IDisposable
    {

        #region Properties

        /// <summary>
        /// The bounded context associated with the database connection.
        /// </summary>
        BoundedContext Context { get; }

        /// <summary>
        /// The shared connection.
        /// </summary>
        DbConnection Connection { get; }

        #endregion Properties

    }
}
