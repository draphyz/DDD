using System.Data.Common;
using System.Collections.Generic;
using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Application;

    internal class SqlServerToCommandExceptionTranslator : IObjectTranslator<DbException, CommandException>
    {
        #region Methods

        public CommandException Translate(DbException exception, IDictionary<string, object> options = null)
        {
            Condition.Requires(exception, nameof(exception)).IsNotNull();
            Condition.Requires(options, nameof(options))
                     .IsNotNull()
                     .Evaluate(options.ContainsKey("Command"));
            var command = (ICommand)options["Command"];
            dynamic sqlServerException = exception;
            foreach (dynamic error in sqlServerException.Errors)
            {
                if (SqlServerErrorHelper.IsUnavailableError(error))
                    return new CommandUnavailableException(command, exception);

                if (SqlServerErrorHelper.IsUnauthorizedError(error))
                    return new CommandUnauthorizedException(command, exception);

                if (SqlServerErrorHelper.IsTimeoutError(error))
                    return new CommandTimeoutException(command, exception);
            }
            return new CommandException(isTransient: false, command, exception);
        }

        #endregion Methods
    }
}
