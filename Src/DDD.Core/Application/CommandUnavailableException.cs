using System;

namespace DDD.Core.Application
{
    /// <summary>
    /// Exception thrown when a command cannot be currently handled.
    /// </summary>
    public class CommandUnavailableException : CommandException
    {

        #region Constructors

        public CommandUnavailableException(ICommand command = null, Exception innerException = null)
            : base(true, DefaultMessage(command), command, innerException)
        {
        }

        public CommandUnavailableException(string message, ICommand command = null, Exception innerException = null)
            : base(true, message, command, innerException)
        {
        }

        #endregion Constructors

        #region Methods

        public static new string DefaultMessage(ICommand command = null)
        {
            if (command == null)
                return "The command cannot be currently handled.";
            return $"The command '{command.GetType().Name}' cannot be currently handled.";
        }

        #endregion Methods

    }
}
