using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    internal static class SqlServerErrorHelper
    {

        #region Methods

        public static bool IsUnavailableError(dynamic error)
        {
            Condition.Requires(error, nameof(error)).IsNotNull();
            switch (error.Number)
            {
                // SQL Error Code: 40613
                // Database XXXX on server YYYY is not currently available. Please retry the connection later. If the problem persists, contact customer 
                // support, and provide them the session tracing ID of ZZZZZ.
                case 40613:
                // SQL Error Code: 40540
                // The service has encountered an error processing your request. Please try again.
                case 40540:
                // SQL Error Code: 40501
                // The service is currently busy. Retry the request after 10 seconds. Code: (reason code to be decoded).
                case 40501:
                // SQL Error Code: 40197
                // The service has encountered an error processing your request. Please try again.
                case 40197:
                // SQL Error Code: 40143
                // The service has encountered an error processing your request. Please try again.
                case 40143:
                // SQL Error Code: 10928
                // Resource ID: %d. The %s limit for the database is %d and has been reached.
                case 10928:
                // SQL Error Code: 10929
                // Resource ID: %d. The %s minimum guarantee is %d, maximum limit is %d and the current usage for the database is %d. 
                // However, the server is currently too busy to support requests greater than %d for this database.
                case 10929:
                // SQL Error Code: 10060
                // A network-related or instance-specific error occurred while establishing a connection to SQL Server. 
                // The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server 
                // is configured to allow remote connections. (provider: TCP Provider, error: 0 - A connection attempt failed 
                // because the connected party did not properly respond after a period of time, or established connection failed 
                // because connected host has failed to respond.)"}
                case 10060:
                // SQL Error Code: 10053
                // A transport-level error has occurred when receiving results from the server.
                // An established connection was aborted by the software in your host machine.
                case 10053:
                // SQL Error Code: 10054
                // A transport-level error has occurred when sending the request to the server. 
                // (provider: TCP Provider, error: 0 - An existing connection was forcibly closed by the remote host.)
                case 10054:
                // SQL Error Code: 233
                // The client was unable to establish a connection because of an error during connection initialization process before login. 
                // Possible causes include the following: the client tried to connect to an unsupported version of SQL Server; the server was too busy 
                // to accept new connections; or there was a resource limitation (insufficient memory or maximum allowed connections) on the server. 
                // (provider: TCP Provider, error: 0 - An existing connection was forcibly closed by the remote host.)
                case 233:
                // SQL Error Code: 64
                // A connection was successfully established with the server, but then an error occurred during the login process. 
                // (provider: TCP Provider, error: 0 - The specified network name is no longer available.) 
                case 64:
                // DBNETLIB Error Code: 20
                // The instance of SQL Server you attempted to connect to does not support encryption.
                case 20:
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
                // SQL Error Code: 40532
                // Cannot open server "%.*ls" requested by the login.  The login failed.
                case 40532:
                // SQL Error Code: 18488
                // Login failed for user '%.*ls'.  Reason: The password of the account must be changed.%.*ls
                case 18488:
                // SQL Error Code: 18487
                // Login failed for user '%.*ls'.  Reason: The password of the account has expired.%.*ls
                case 18487:
                // SQL Error Code: 18486
                // Login failed for user '%.*ls' because the account is currently locked out. The system administrator can unlock it. %.*ls
                case 18486:
                // SQL Error Code: 18470
                // Login failed for user '%.*ls'. Reason: The account is disabled.%.*ls
                case 18470:
                // SQL Error Code: 18456
                // Login failed for user '%.*ls'.%.*ls%.*
                case 18456:
                // SQL Error Code: 18452
                // Login failed. The login is from an untrusted domain and cannot be used with Windows authentication.%.*ls
                case 18452:
                // SQL Error Code: 4060
                // Cannot open database "%.*ls" requested by the login. The login failed.
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
                // DBNETLIB Error Code: -2
                // Timeout expired. The timeout period elapsed prior to completion of the operation or the server is not responding.
                case -2:
                    return true;
                default:
                    return false;
            }
        }

        #endregion Methods

    }
}
