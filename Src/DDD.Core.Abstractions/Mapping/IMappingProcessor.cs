using System.Collections.Generic;

namespace DDD.Mapping
{
    /// <summary>
    /// Defines a component that processes object-object mappings.
    /// </summary>
    public interface IMappingProcessor
    {

        #region Methods

        void Map<TSource, TDestination>(TSource source, TDestination destination, IDictionary<string, object> options = null)
            where TSource : class
            where TDestination : class;

        TDestination Translate<TDestination>(object source, IDictionary<string, object> options = null)
            where TDestination : class;

        TDestination Translate<TSource, TDestination>(TSource source, IDictionary<string, object> options = null)
            where TSource : class
            where TDestination : class;

        #endregion Methods

    }
}