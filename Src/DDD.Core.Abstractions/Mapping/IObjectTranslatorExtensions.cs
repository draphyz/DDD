using Conditions;
using System.Collections.Generic;

namespace DDD.Mapping
{
    using Collections;

    public static class IObjectTranslatorExtensions
    {

        #region Methods

        public static TDestination Translate<TSource, TDestination>(this IObjectTranslator<TSource, TDestination> translator,
                                                                    TSource source,
                                                                    object options)
            where TSource : class
            where TDestination : class
        {
            Condition.Requires(translator, nameof(translator)).IsNotNull();
            var dictionary = new Dictionary<string, object>();
            dictionary.AddObject(options);
            return translator.Translate(source, dictionary);
        }

        public static IEnumerable<TDestination> TranslateCollection<TSource, TDestination>(this IObjectTranslator<TSource, TDestination> translator,
                                                                                           IEnumerable<TSource> source,
                                                                                           IDictionary<string, object> options = null)
            where TSource : class
            where TDestination : class
        {
            Condition.Requires(translator, nameof(translator)).IsNotNull();
            foreach (var item in source)
                yield return translator.Translate(item, options);
        }

        public static IEnumerable<TDestination> TranslateCollection<TSource, TDestination>(this IObjectTranslator<TSource, TDestination> translator,
                                                                                           IEnumerable<TSource> source,
                                                                                           object options)
            where TSource : class
            where TDestination : class
        {
            Condition.Requires(translator, nameof(translator)).IsNotNull();
            var dictionary = new Dictionary<string, object>();
            dictionary.AddObject(options);
            return translator.TranslateCollection(source, dictionary);
        }

        #endregion Methods

    }
}
