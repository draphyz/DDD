using System.Threading.Tasks;

namespace DDD.Validation
{
    /// <summary>
    /// Defines a method that validates asynchronously an object of a specified type.
    /// </summary>
    public interface IAsyncObjectValidator<in T> where T :class
    {

        #region Methods

        /// <summary>
        /// Validates asynchronously an object of a specified type.
        /// </summary>
        Task<ValidationResult> ValidateAsync(T obj, IValidationContext context);

        #endregion Methods

    }
}
