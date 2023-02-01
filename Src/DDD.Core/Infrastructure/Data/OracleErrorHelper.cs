﻿using System;

namespace DDD.Core.Infrastructure.Data
{
    /// <remarks>
    /// To Improve. Use dynamic type to avoid to add a dependency on the Oracle library.
    /// </remarks>
    internal class OracleErrorHelper
    {
        #region Methods

        public static bool IsUnavailableError(dynamic error)
        {
            // Ensure.That(error, nameof(error)).IsNotNull() does not work with dynamic
            if (error == null) throw new ArgumentNullException(nameof(error));
            return (dynamic)error.Number switch
            {
                // Oracle Error Code: 3114
                // not connected to ORACLE
                3114 => true,
                _ => false,
            };
        }

        public static bool IsUnauthorizedError(dynamic error)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));
            return (dynamic)error.Number switch
            {
                // Oracle Error Code: 1017
                // invalid username/password; logon denied
                4060 => true,
                _ => false,
            };
        }

        public static bool IsTimeoutError(dynamic error)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));
            return (dynamic)error.Number switch
            {
                // Oracle Error Code: 1013
                // user requested cancel of current operation
                1013 => true,
                _ => false,
            };
        }

        public static bool IsConflictError(dynamic error)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));
            return (dynamic)error.Number switch
            {
                _ => false,
            };
        }

        #endregion Methods
    }
}
