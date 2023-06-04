namespace DDD.Mapping
{
    /// <summary>
    /// Defines a method that translates an input object of one type into an output object of a different type.
    /// </summary>
    public interface IObjectTranslator<in TSource, out TDestination> : IObjectTranslator
        where TSource : class
        where TDestination : class
    {
        #region Methods

        TDestination Translate(TSource source, IMappingContext context);

        #endregion Methods
    }
}
