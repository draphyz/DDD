﻿using System.Data.Common;
using EnsureThat;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Application;
    using Collections;

    internal class SqlServerToCommandExceptionTranslator : ObjectTranslator<DbException, CommandException>
    {
        #region Methods

        public override CommandException Translate(DbException exception, IMappingContext context)
        {
            Ensure.That(exception, nameof(exception)).IsNotNull();
            Ensure.That(context, nameof(context)).IsNotNull();
            context.TryGetValue("Command", out ICommand command);
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
