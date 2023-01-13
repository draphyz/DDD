using System;

namespace DDD.Core.Application
{
    public class MarkRecurringCommandAsFailed : ICommand
    {
        #region Properties

        public Guid CommandId { get; set; }

        public DateTime ExecutionTime { get; set; }

        public string ExceptionInfo { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
            => $"{this.GetType().Name} [ {nameof(CommandId)}={this.CommandId}, {nameof(ExecutionTime)}={this.ExecutionTime}]";

        #endregion Methods
    }
}