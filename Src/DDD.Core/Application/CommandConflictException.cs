using System;

namespace DDD.Core.Application
{
    /// <summary>
    /// Exception thrown when a conflict with other commands has been detected while handling a command.
    /// </summary>
    public class CommandConflictException : CommandException
    {

        #region Constructors

        public CommandConflictException(ICommand command = null, Exception innerException = null)
            : base(true, DefaultMessage(command), command, innerException)
        {
        }

        public CommandConflictException(string message, ICommand command = null, Exception innerException = null)
            : base(true, message, command, innerException)
        {
        }

        #endregion Constructors

        #region Methods

        public static new string DefaultMessage(ICommand command = null)
        {
            if (command == null)
                return "A conflict has been detected while handling a command.";
            return $"A conflict has been detected while handling the command '{command.GetType().Name}'.";
        }

        #endregion Methods

    }
}
