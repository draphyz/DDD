using System.Collections.Generic;
using Conditions;

namespace DDD.Core.Application
{
    using Mapping;
    using Domain;

    internal class DomainToCommandExceptionTranslator : IObjectTranslator<DomainException, CommandException>
    {

        #region Fields

        public static readonly IObjectTranslator<DomainException, CommandException> Default = new DomainToCommandExceptionTranslator();

        #endregion Fields

        #region Methods

        public CommandException Translate(DomainException exception, IDictionary<string, object> options = null)
        {
            Condition.Requires(exception, nameof(exception)).IsNotNull();
            Condition.Requires(options, nameof(options))
                     .IsNotNull()
                     .Evaluate(options.ContainsKey("Command"));
            var command = (ICommand)options["Command"];
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
                    return new CommandException(exception.IsTransient, command, exception);
            }
        }

        #endregion Methods

    }
}
