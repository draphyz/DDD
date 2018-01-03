namespace DDD.Core.Infrastructure.Validation.Validators
{
    internal class MaximumCountValidator : CountValidator
    {
        public MaximumCountValidator(int max) :
            base(0, max, "'{PropertyName}' must contain no more than {Max} item(s). '{PropertyName}' has {Count} item(s).")
        {
        }
    }
}
