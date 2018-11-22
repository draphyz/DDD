using System.Linq;
using System.Collections;
using FluentValidation.Validators;
using Conditions;

namespace DDD.Core.Infrastructure.Validation.Validators
{
    internal class CountValidator : PropertyValidator
    {
        public int Min { get; }

        public int? Max { get; }

        public CountValidator(int min, int? max, string errorMessage) : base(errorMessage)
        {
            Condition.Requires(min, nameof(min)).IsGreaterOrEqual(0);
            if (max.HasValue)
                Condition.Requires(max, nameof(max)).IsGreaterOrEqual(min);
            this.Min = min;
            this.Max = max;
        }

        public CountValidator(int min, int? max) 
            : this(min, max, "'{PropertyName}' must contain between {Min} and {Max} items. '{PropertyName}' has {Count} items.")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null) return true;
            var collection = context.PropertyValue as IEnumerable;
            if (collection == null) return true;
            var count = collection.Cast<object>().Count();
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
    }
}
