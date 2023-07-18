using System;

namespace DDD.Core.Application
{
    public class RecurringCommandDetail
    {
        #region Properties

        public Guid CommandId { get; set; }

        public string CommandType { get; set; }

        public string Body { get; set; }

        public string BodyFormat { get; set; }

        public string RecurringExpression { get; set; }

        public string RecurringExpressionFormat { get; set; }

        public DateTime? LastExecutionTime { get; set; }

        public CommandExecutionStatus? LastExecutionStatus { get; set; }

        public string LastExceptionInfo { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
            => $"{this.GetType().Name} [ {nameof(CommandId)}={this.CommandId}, {nameof(CommandType)}={this.CommandType}, {nameof(RecurringExpression)}={this.RecurringExpression}, {nameof(RecurringExpressionFormat)}={this.RecurringExpressionFormat}]";

        #endregion Methods

    }
}
