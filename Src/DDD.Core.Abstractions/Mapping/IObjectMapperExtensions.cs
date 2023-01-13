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
                                                      object context)
            where TSource : class
            where TDestination : class
        {
            Condition.Requires(mapper, nameof(mapper)).IsNotNull();
            var dictionary = new Dictionary<string, object>();
            dictionary.AddObject(context);
            mapper.Map(source, destination, dictionary);
        }

        #endregion Methods

    }
}
