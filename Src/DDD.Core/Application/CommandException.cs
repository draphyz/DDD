using System;

namespace DDD.Core.Application
{
    /// <summary>
    /// Exception thrown when a command failed.
    /// </summary>
    public class CommandException : ApplicationException
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

        public CommandException(Exception innerException, ICommand Command)
            : base($"The command '{Command.GetType().Name}' failed.", innerException)
        {
            this.Command = Command;
        }

        #endregion Constructors

        #region Properties

        public ICommand Command { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            var s = $"{this.GetType()}: {this.Message} ";
            if (this.Command != null)
                s += $"{Environment.NewLine}Command: {this.Command}";
            if (this.InnerException != null)
                s += $" ---> {this.InnerException}";
            if (this.StackTrace != null)
                s += $"{Environment.NewLine}{this.StackTrace}";
            return s;
        }

        #endregion Methods

    }
}
