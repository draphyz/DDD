using System.Data.Common;
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

        public override CommandException Translate(DbException exception, IMappingContext context)
        {
            Ensure.That(exception, nameof(exception)).IsNotNull();
            Ensure.That(context, nameof(context)).IsNotNull();
            context.TryGetValue("Command", out ICommand command);
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
