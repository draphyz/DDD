using System;

namespace DDD.Core.Application
{
    public class RegisterRecurringCommand : ICommand
    {
        #region Properties

        public Guid CommandId { get; set; }

        public string CommandType { get; set; }

        public string Body { get; set; }

        public string BodyFormat { get; set; }

        public string CronExpression { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
            => $"{this.GetType().Name} [ {nameof(CommandId)}={this.CommandId}, {nameof(CommandType)}={this.CommandType}, {nameof(CronExpression)}={this.CronExpression}]";

        #endregion Methods
    }
}
