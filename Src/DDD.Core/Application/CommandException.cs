using System;

namespace DDD.Core.Application
{
    /// <summary>
    /// Exception thrown when a command failed.
    /// </summary>
    public class CommandException : ApplicationException
    {

        #region Constructors

        public CommandException(bool isTransient, ICommand command = null, Exception innerException = null)
            : base(isTransient, DefaultMessage(command), innerException)
        {
            this.Command = command;
        }

        public CommandException(bool isTransient, string message, ICommand command = null, Exception innerException = null)
            : base(isTransient, message, innerException)
        {
            this.Command = command;
        }

        #endregion Constructors

        #region Properties

        public ICommand Command { get; }

        #endregion Properties

        #region Methods

        public static string DefaultMessage(ICommand command = null)
        {
            if (command == null)
                return "A command failed.";
            return $"The command '{command.GetType().Name}' failed.";
        }

        public override string ToString()
        {
            var s = $"{this.GetType()}: {this.Message} ";
            s += $"{Environment.NewLine}IsTransient: {this.IsTransient}";
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
