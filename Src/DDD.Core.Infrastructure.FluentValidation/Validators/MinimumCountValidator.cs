namespace DDD.Core.Infrastructure.Validation.Validators
{
    internal class MinimumCountValidator : CountValidator
    {
        public MinimumCountValidator(int min) :
            base(min, null, "'{PropertyName}' must contain at least {Min} item(s). '{PropertyName}' has {Count} item(s).")
        {
        }
    }
}
