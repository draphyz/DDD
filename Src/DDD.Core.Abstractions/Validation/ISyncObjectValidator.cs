namespace DDD.Validation
{
    /// <summary>
    /// Defines a method that validates synchronously an object of a specified type.
    /// </summary>
    public interface ISyncObjectValidator<in T> where T :class
    {

        #region Methods

        /// <summary>
        /// Validates synchronously an object of a specified type.
        /// </summary>
        ValidationResult Validate(T obj, string ruleSet = null);

        #endregion Methods

    }
}
