namespace DDD.Core.Mapping
{
    /// <summary>
    /// Defines a component that processes object-object mappings.
    /// </summary>
    public interface IMappingProcessor
    {

        #region Methods

        void Map<TSource, TDestination>(TSource source, TDestination destination) where TSource : class
                                                                                  where TDestination : class;

        TDestination Translate<TDestination>(object source) where TDestination : class;

        TDestination Translate<TSource, TDestination>(TSource source) where TSource : class
                                                                      where TDestination : class;

        #endregion Methods

    }
}