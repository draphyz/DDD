using EnsureThat;

namespace DDD.Validation
{
    public static class ISyncObjectValidatorExtensions
    {

        #region Methods

        public static ValidationResult Validate<T>(this ISyncObjectValidator<T> validator, 
                                                   T obj)
            where T : class
        {
            Ensure.That(validator, nameof(validator)).IsNotNull();
            return validator.Validate(obj, new ValidationContext());
        }

        public static ValidationResult Validate<T>(this ISyncObjectValidator<T> validator, 
                                                   T obj, 
                                                   object context)
            where T : class
        {
            Ensure.That(validator, nameof(validator)).IsNotNull();
            return validator.Validate(obj, ValidationContext.FromObject(context));
        }

        #endregion Methods

    }
}
