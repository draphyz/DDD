namespace DDD.Validation
{
    /// <summary>
    /// Defines a method that validates an object of a specified type.
    /// </summary>
    public interface IObjectValidator<in T> where T :class
    {

        #region Methods

        ValidationResult Validate(T obj, string ruleSet = null);

        #endregion Methods

    }
}
