using EnsureThat;
using System.Threading.Tasks;

namespace DDD.Validation
{
    public static class IAsyncObjectValidatorExtensions
    {

        #region Methods

        public static Task<ValidationResult> ValidateAsync<T>(this IAsyncObjectValidator<T> validator, 
                                                              T obj)
            where T : class
        {
            Ensure.That(validator, nameof(validator)).IsNotNull();
            return validator.ValidateAsync(obj, new ValidationContext());
        }

        public static Task<ValidationResult> ValidateAsync<T>(this IAsyncObjectValidator<T> validator, 
                                                              T obj, 
                                                              object context)
            where T : class
        {
            Ensure.That(validator, nameof(validator)).IsNotNull();
            return validator.ValidateAsync(obj, ValidationContext.FromObject(context));
        }

        #endregion Methods

    }
}
