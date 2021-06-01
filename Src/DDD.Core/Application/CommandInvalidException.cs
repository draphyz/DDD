using System;
using System.Linq;
using DDD.Validation;

namespace DDD.Core.Application
{
    /// <summary>
    /// Exception thrown when a command is invalid.
    /// </summary>
    public class CommandInvalidException : CommandException
    {

        #region Constructors

        public CommandInvalidException(ICommand command = null, ValidationFailure[] failures = null, Exception innerException = null)
            : base(false, DefaultMessage(command), command, innerException)
        {
            this.Failures = failures;
        }

        public CommandInvalidException(string message, ICommand command = null, ValidationFailure[] failures = null, Exception innerException = null)
            : base(false, message, command, innerException)
        {
            this.Failures = failures;
        }

        #endregion Constructors

        #region Properties

        public ValidationFailure[] Failures { get; }

        #endregion Properties

        #region Methods

        public static new string DefaultMessage(ICommand command = null)
        {
            if (command == null)
                return "The command is invalid.";
            return $"The command '{command.GetType().Name}' is invalid.";
        }

        public bool HasFailures() => this.Failures != null && this.Failures.Any();

        public override string ToString()
        {
            var s = $"{this.GetType()}: {this.Message} ";
            s += $"{Environment.NewLine}IsTransient: {this.IsTransient}";
            if (this.Command != null)
                s += $"{Environment.NewLine}Command: {this.Command}";
            if (this.Failures != null)
            {
                for (var i = 0; i < this.Failures.Length; i++)
                    s += $"{Environment.NewLine}Failure{i}: {this.Failures[i]}";
            }
            if (this.InnerException != null)
                s += $" ---> {this.InnerException}";
            if (this.StackTrace != null)
                s += $"{Environment.NewLine}{this.StackTrace}";
            return s;
        }

        #endregion Methods

    }
}
