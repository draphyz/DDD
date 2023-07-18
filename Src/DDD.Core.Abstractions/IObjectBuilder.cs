namespace DDD
{
    /// <summary>
    /// Defines a method that builds an object of a specified type.
    /// </summary>
    public interface IObjectBuilder<out T> 
        where T : class
    {
        #region Methods

        T Build();

        #endregion Methods
    }
}