using System;

namespace DDD.Core.Application
{
    /// <summary>
    /// Exception thrown when a command failed.
    /// </summary>
    public class CommandException : Exception
    {

        #region Constructors

        public CommandException() 
            : base("The command failed.")
        {
        }

        public CommandException(string message) : base(message)
        {
        }

        public CommandException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CommandException(string message, Exception innerException, ICommand command) : base(message, innerException)
        {
            this.Command = command;
        }

        #endregion Constructors

        #region Properties

        public ICommand Command { get; set; }

        #endregion Properties

    }
}
