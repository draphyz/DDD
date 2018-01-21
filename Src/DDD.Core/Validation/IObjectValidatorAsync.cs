using System.Threading.Tasks;

namespace DDD.Core.Validation
{
    /// <summary>
    /// Defines a method that validates asynchronously an object of a specified type.
    /// </summary>
    public interface IObjectValidatorAsync<in T> where T :class
    {

        #region Methods

        Task<ValidationResult> ValidateAsync(T obj, string ruleSet = null);

        #endregion Methods

    }
}
