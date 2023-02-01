namespace DDD.Core.Infrastructure.Data
{
    /// <summary>
    /// Defines a generator of data values of a particular type.
    /// </summary>
    /// <typeparam name="TValue">The type of data values.</typeparam>
    public interface IValueGenerator<out TValue>
    {
        #region Methods

        /// <summary>
        /// Generates data values.
        /// </summary>
        TValue Generate();

        #endregion Methods
    }
}