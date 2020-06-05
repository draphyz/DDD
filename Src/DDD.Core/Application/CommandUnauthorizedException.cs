using System;

namespace DDD.Core.Application
{
    /// <summary>
    /// Exception thrown when a command was denied.
    /// </summary>
    public class CommandUnauthorizedException : CommandException
    {

        #region Constructors

        public CommandUnauthorizedException(ICommand command = null, Exception innerException = null)
            : base(false, DefaultMessage(command), command, innerException)
        {
        }

        public CommandUnauthorizedException(string message, ICommand command = null, Exception innerException = null)
            : base(false, message, command, innerException)
        {
        }

        #endregion Constructors

        #region Methods

        public static new string DefaultMessage(ICommand command = null)
        {
            if (command == null)
                return "The command was denied.";
            return $"The command '{command.GetType().Name}' was denied.";
        }

        #endregion Methods

    }
}
