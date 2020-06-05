using System;

namespace DDD.Core.Application
{
    /// <summary>
    /// Exception thrown when a command has expired.
    /// </summary>
    public class CommandTimeoutException : CommandException
    {

        #region Constructors

        public CommandTimeoutException(ICommand command = null, Exception innerException = null)
            : base(true, DefaultMessage(command), command, innerException)
        {
        }

        public CommandTimeoutException(string message, ICommand command = null, Exception innerException = null)
            : base(true, message, command, innerException)
        {
        }

        #endregion Constructors

        #region Methods

        public static new string DefaultMessage(ICommand command = null)
        {
            if (command == null)
                return "The command has expired.";
            return $"The command '{command.GetType().Name}' has expired.";
        }

        #endregion Methods

    }
}
