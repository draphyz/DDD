using EnsureThat;

namespace DDD.Mapping
{
    public static class IObjectMapperExtensions
    {

        #region Methods

        public static void Map<TSource, TDestination>(this IObjectMapper<TSource, TDestination> mapper,
                                                      TSource source,
                                                      TDestination destination)
            where TSource : class
            where TDestination : class
        {
            Ensure.That(mapper, nameof(mapper)).IsNotNull();
            mapper.Map(source, destination, new MappingContext());
        }

        public static void Map<TSource, TDestination>(this IObjectMapper<TSource, TDestination> mapper,
                                                      TSource source,
                                                      TDestination destination,
                                                      object context)
            where TSource : class
            where TDestination : class
        {
            Ensure.That(mapper, nameof(mapper)).IsNotNull();
            mapper.Map(source, destination, MappingContext.FromObject(context));
        }

        #endregion Methods

    }
}
