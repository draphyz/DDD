using System.Data.Common;
using System.Collections.Generic;
using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Application;
    using Collections;

    internal class SqlServerToCommandExceptionTranslator : ObjectTranslator<DbException, CommandException>
    {
        #region Methods

        public override CommandException Translate(DbException exception, IDictionary<string, object> context = null)
        {
            Condition.Requires(exception, nameof(exception)).IsNotNull();
            ICommand command = null;
            context?.TryGetValue("Command", out command);
            dynamic sqlServerException = exception;
            foreach (dynamic error in sqlServerException.Errors)
            {
                if (SqlServerErrorHelper.IsUnavailableError(error))
                    return new CommandUnavailableException(command, exception);

                if (SqlServerErrorHelper.IsUnauthorizedError(error))
                    return new CommandUnauthorizedException(command, exception);

                if (SqlServerErrorHelper.IsTimeoutError(error))
                    return new CommandTimeoutException(command, exception);

                if (SqlServerErrorHelper.IsConflictError(error))
                    return new CommandConflictException(command, exception);
            }
            return new CommandException(isTransient: false, command, exception);
        }

        #endregion Methods
    }
}
