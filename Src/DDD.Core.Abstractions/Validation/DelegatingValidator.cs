using System;
using System.Threading.Tasks;
using EnsureThat;

namespace DDD.Validation
{
    /// <summary>
    /// Adapter that converts a delegate into an object that implements the interface IObjectValidator.
    /// </summary>
    public class DelegatingValidator<T> : IObjectValidator<T>
        where T : class
    {

        #region Fields

        private readonly Func<T, IValidationContext, ValidationResult> syncValidator;
        private readonly Func<T, IValidationContext, Task<ValidationResult>> asyncValidator;

        #endregion Fields

        #region Constructors

        public DelegatingValidator(Func<T, IValidationContext, ValidationResult> syncValidator, 
                                         Func<T, IValidationContext, Task<ValidationResult>> asyncValidator) 
        {
            Ensure.That(syncValidator, nameof(syncValidator)).IsNotNull();
            Ensure.That(asyncValidator, nameof(asyncValidator)).IsNotNull();
            this.syncValidator = syncValidator;
            this.asyncValidator = asyncValidator;
        }

        #endregion Constructors

        #region Methods

        public static IObjectValidator<T> Create(Func<T, IValidationContext, ValidationResult> syncValidator)
        {
            Func<T, IValidationContext, Task<ValidationResult>> asyncValidator = (obj, context) => Task.FromResult(syncValidator(obj, context));
            return new DelegatingValidator<T>(syncValidator, asyncValidator);
        }

        public ValidationResult Validate(T obj, IValidationContext context)
        {
            Ensure.That(obj, nameof(obj)).IsNotNull();
            Ensure.That(context, nameof(context)).IsNotNull();
            return this.syncValidator(obj, context);
        }

        public Task<ValidationResult> ValidateAsync(T obj, IValidationContext context)
        {
            Ensure.That(obj, nameof(obj)).IsNotNull();
            Ensure.That(context, nameof(context)).IsNotNull();
            return this.asyncValidator(obj, context);
        }

        #endregion Methods

    }
}
