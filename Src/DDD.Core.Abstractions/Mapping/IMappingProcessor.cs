namespace DDD.Mapping
{
    /// <summary>
    /// Defines a component that processes object-object mappings.
    /// </summary>
    public interface IMappingProcessor
    {

        #region Methods

        void Map<TSource, TDestination>(TSource source, TDestination destination, IMappingContext context) 
            where TSource : class
            where TDestination : class;

        TDestination Translate<TDestination>(object source, IMappingContext context) 
            where TDestination : class;

        TDestination Translate<TSource, TDestination>(TSource source, IMappingContext context) 
            where TSource : class
            where TDestination : class;

        #endregion Methods

    }
}