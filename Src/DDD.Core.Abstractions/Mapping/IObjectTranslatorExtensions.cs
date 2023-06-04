using EnsureThat;
using System.Collections.Generic;

namespace DDD.Mapping
{
    public static class IObjectTranslatorExtensions
    {

        #region Methods

        public static TDestination Translate<TSource, TDestination>(this IObjectTranslator<TSource, TDestination> translator,
                                                                    TSource source)
            where TSource : class
            where TDestination : class
        {
            Ensure.That(translator, nameof(translator)).IsNotNull();
            return translator.Translate(source, new MappingContext());
        }

        public static TDestination Translate<TSource, TDestination>(this IObjectTranslator<TSource, TDestination> translator,
                                                                    TSource source,
                                                                    object context)
            where TSource : class
            where TDestination : class
        {
            Ensure.That(translator, nameof(translator)).IsNotNull();
            return translator.Translate(source, MappingContext.FromObject(context));
        }

        public static IEnumerable<TDestination> TranslateCollection<TSource, TDestination>(this IObjectTranslator<TSource, TDestination> translator,
                                                                                           IEnumerable<TSource> source)
            where TSource : class
            where TDestination : class
        {
            Ensure.That(translator, nameof(translator)).IsNotNull();
            return translator.TranslateCollection(source, new MappingContext());
        }

        public static IEnumerable<TDestination> TranslateCollection<TSource, TDestination>(this IObjectTranslator<TSource, TDestination> translator,
                                                                                           IEnumerable<TSource> source,
                                                                                           IMappingContext context)
            where TSource : class
            where TDestination : class
        {
            Ensure.That(translator, nameof(translator)).IsNotNull();
            Ensure.That(context, nameof(context)).IsNotNull();
            foreach (var item in source)
                yield return translator.Translate(item, context);
        }

        public static IEnumerable<TDestination> TranslateCollection<TSource, TDestination>(this IObjectTranslator<TSource, TDestination> translator,
                                                                                           IEnumerable<TSource> source,
                                                                                           object context)
            where TSource : class
            where TDestination : class
        {
            Ensure.That(translator, nameof(translator)).IsNotNull();
            return translator.TranslateCollection(source, MappingContext.FromObject(context));
        }

        #endregion Methods

    }
}
