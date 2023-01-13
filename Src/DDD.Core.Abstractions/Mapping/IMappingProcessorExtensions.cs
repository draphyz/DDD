using Conditions;
using System.Collections.Generic;

namespace DDD.Mapping
{
    using Collections;

    public static class IMappingProcessorExtensions
    {

        #region Methods

        public static void Map<TSource, TDestination>(this IMappingProcessor processor,
                                                      TSource source,
                                                      TDestination destination,
                                                      object context)
            where TSource : class
            where TDestination : class
        {
            Condition.Requires(processor, nameof(processor)).IsNotNull();
            var dictionary = new Dictionary<string, object>();
            dictionary.AddObject(context);
            processor.Map(source, destination, dictionary);
        }

        public static TDestination Translate<TDestination>(this IMappingProcessor processor,
                                                           object source,
                                                           object context)
            where TDestination : class
        {
            Condition.Requires(processor, nameof(processor)).IsNotNull();
            var dictionary = new Dictionary<string, object>();
            dictionary.AddObject(context);
            return processor.Translate<TDestination>(source, dictionary);
        }

        public static TDestination Translate<TSource, TDestination>(this IMappingProcessor processor,
                                                                    TSource source,
                                                                    object context)
            where TSource : class
            where TDestination : class
        {
            Condition.Requires(processor, nameof(processor)).IsNotNull();
            var dictionary = new Dictionary<string, object>();
            dictionary.AddObject(context);
            return processor.Translate<TSource, TDestination>(source, dictionary);
        }

        public static IEnumerable<TDestination> TranslateCollection<TSource, TDestination>(this IMappingProcessor processor,
                                                                                           IEnumerable<TSource> source,
                                                                                           IDictionary<string, object> context = null)
            where TSource : class
            where TDestination : class
        {
            Condition.Requires(processor, nameof(processor)).IsNotNull();
            foreach (var item in source)
                yield return processor.Translate<TSource, TDestination>(item, context);
        }

        public static IEnumerable<TDestination> TranslateCollection<TDestination>(this IMappingProcessor processor,
                                                                                  IEnumerable<object> source,
                                                                                  IDictionary<string, object> context = null)
            where TDestination : class
        {
            Condition.Requires(processor, nameof(processor)).IsNotNull();
            foreach (var item in source)
                yield return processor.Translate<TDestination>(item, context);
        }

        public static IEnumerable<TDestination> TranslateCollection<TSource, TDestination>(this IMappingProcessor processor,
                                                                                           IEnumerable<TSource> source,
                                                                                           object context)
            where TSource : class
            where TDestination : class
        {
            Condition.Requires(processor, nameof(processor)).IsNotNull();
            var dictionary = new Dictionary<string, object>();
            dictionary.AddObject(context);
            return processor.TranslateCollection<TSource, TDestination>(source, dictionary);
        }

        public static IEnumerable<TDestination> TranslateCollection<TDestination>(this IMappingProcessor processor,
                                                                                  IEnumerable<object> source,
                                                                                  object context)
            where TDestination : class
        {
            Condition.Requires(processor, nameof(processor)).IsNotNull();
            var dictionary = new Dictionary<string, object>();
            dictionary.AddObject(context);
            return processor.TranslateCollection<TDestination>(source, dictionary);
        }

        #endregion Methods

    }
}
