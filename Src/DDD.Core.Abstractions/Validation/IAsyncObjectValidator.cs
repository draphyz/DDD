using System.Threading;
using System.Threading.Tasks;

namespace DDD.Validation
{
    /// <summary>
    /// Defines a method that validates asynchronously an object of a specified type.
    /// </summary>
    public interface IAsyncObjectValidator<in T> where T :class
    {

        #region Methods

        Task<ValidationResult> ValidateAsync(T obj, string ruleSet = null, CancellationToken cancellationToken = default);

        #endregion Methods

    }
}
