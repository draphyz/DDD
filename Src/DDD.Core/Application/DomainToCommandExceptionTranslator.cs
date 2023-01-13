using Conditions;
using System.Collections.Generic;

namespace DDD.Core.Application
{
    using Mapping;
    using Domain;
    using Collections;

    public class DomainToCommandExceptionTranslator : ObjectTranslator<DomainException, CommandException>
    {

        #region Methods

        public override CommandException Translate(DomainException exception, IDictionary<string, object> context = null)
        {
            Condition.Requires(exception).IsNotNull();
            ICommand command = null;
            context?.TryGetValue("Command", out command);
            switch (exception)
            {
                case DomainServiceConflictException _:
                case RepositoryConflictException _:
                    return new CommandConflictException(command, exception);
                case DomainServiceTimeoutException _:
                case RepositoryTimeoutException _:
                    return new CommandTimeoutException(command, exception);
                case DomainServiceUnauthorizedException _:
                case RepositoryUnauthorizedException _:
                    return new CommandUnauthorizedException(command, exception);
                case DomainServiceUnavailableException _:
                case RepositoryUnavailableException _:
                    return new CommandUnavailableException(command, exception);
                case DomainServiceInvalidException invalidEx:
                    return new CommandInvalidException(command, invalidEx.Failures, exception);
                default:
                    return new CommandException(isTransient: false, command, exception);
            };
        }

        #endregion Methods

    }
}
