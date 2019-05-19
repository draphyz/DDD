using Conditions;
using System.Collections.Generic;

namespace DDD.Mapping
{
    using Collections;

    public static class IObjectMapperExtensions
    {

        #region Methods

        public static void Map<TSource, TDestination>(this IObjectMapper<TSource, TDestination> mapper,
                                                      TSource source,
                                                      TDestination destination,
                                                      object options)
            where TSource : class
            where TDestination : class
        {
            Condition.Requires(mapper, nameof(mapper)).IsNotNull();
            var dictionary = new Dictionary<string, object>();
            dictionary.AddObject(options);
            mapper.Map(source, destination, dictionary);
        }

        #endregion Methods

    }
}
