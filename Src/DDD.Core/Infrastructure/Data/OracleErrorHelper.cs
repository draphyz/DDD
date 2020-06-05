using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    /// <remarks>
    /// To Improve.
    /// </remarks>
    internal class OracleErrorHelper
    {
        #region Methods

        public static bool IsUnavailableError(dynamic error)
        {
            Condition.Requires(error, nameof(error)).IsNotNull();
            switch (error.Number)
            {
                // Oracle Error Code: 3114
                // not connected to ORACLE
                case 3114:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsUnauthorizedError(dynamic error)
        {
            Condition.Requires(error, nameof(error)).IsNotNull();
            switch (error.Number)
            {
                // Oracle Error Code: 1017
                // invalid username/password; logon denied
                case 4060:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsTimeoutError(dynamic error)
        {
            Condition.Requires(error, nameof(error)).IsNotNull();
            switch (error.Number)
            {
                // Oracle Error Code: 1013
                // user requested cancel of current operation
                case 1013:
                    return true;
                default:
                    return false;
            }
        }

        #endregion Methods
    }
}
