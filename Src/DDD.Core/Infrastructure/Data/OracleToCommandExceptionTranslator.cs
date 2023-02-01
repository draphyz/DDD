using System.Data.Common;
using System.Collections.Generic;
using EnsureThat;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Application;
    using Collections;

    /// <remarks>
    /// Use dynamic type to avoid to add a dependency on the Oracle library.
    /// </remarks>
    internal class OracleToCommandExceptionTranslator : ObjectTranslator<DbException, CommandException>
    {
        #region Methods

        public override CommandException Translate(DbException exception, IDictionary<string, object> context = null)
        {
            Ensure.That(exception, nameof(exception)).IsNotNull();
            ICommand command = null;
            context?.TryGetValue("Command", out command);
            dynamic oracleException = exception;
            foreach (dynamic error in oracleException.Errors)
            {
                if (OracleErrorHelper.IsUnavailableError(error))
                    return new CommandUnavailableException(command, exception);

                if (OracleErrorHelper.IsUnauthorizedError(error))
                    return new CommandUnauthorizedException(command, exception);

                if (OracleErrorHelper.IsTimeoutError(error))
                    return new CommandTimeoutException(command, exception);

                if (OracleErrorHelper.IsConflictError(error))
                    return new CommandConflictException(command, exception);
            }
            return new CommandException(isTransient: false, command, exception);
        }

        #endregion Methods
    }
}
