using System.Linq;
using System.Collections;
using FluentValidation;
using FluentValidation.Validators;
using Conditions;

namespace DDD.Core.Infrastructure.Validation.Validators
{
    internal class CountValidator<T> : PropertyValidator<T, IEnumerable>
    {

        #region Constructors

        public CountValidator(int min, int? max)
        {
            Condition.Requires(min, nameof(min)).IsGreaterOrEqual(0);
            if (max.HasValue)
                Condition.Requires(max, nameof(max)).IsGreaterOrEqual(min);
            this.Min = min;
            this.Max = max;
        }

        #endregion Constructors

        #region Properties

        public int? Max { get; }
        public int Min { get; }
        public override string Name => "CountValidator";

        #endregion Properties

        #region Methods

        public override bool IsValid(ValidationContext<T> context, IEnumerable value)
        {
            if (value == null) return true;
            var count = value.Cast<object>().Count();
            if (count < this.Min || (this.Max.HasValue && count > this.Max))
            {
                context.MessageFormatter.AppendArgument("Count", count);
                context.MessageFormatter.AppendArgument("Min", this.Min);
                if (this.Max.HasValue)
                    context.MessageFormatter.AppendArgument("Max", this.Max);
                return false;
            }
            return true;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' must contain between {Min} and {Max} items. '{PropertyName}' has {Count} items.";

        #endregion Methods

    }
}
