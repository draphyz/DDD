namespace DDD.Core.Infrastructure.Validation.Validators
{
    internal class ExactCountValidator : CountValidator
    {
        public ExactCountValidator(int count) :
            base(count, count, "'{PropertyName}' must contain {Max} item(s). '{PropertyName}' has {Count} item(s).")
        {
        }
    }
}
