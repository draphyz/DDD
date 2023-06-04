using EnsureThat;
using System.Collections.Generic;

namespace DDD.Mapping
{
    public static class IMappingProcessorExtensions
    {

        #region Methods
        public static void Map<TSource, TDestination>(this IMappingProcessor processor,
                                                      TSource source,
                                                      TDestination destination)
            where TSource : class
            where TDestination : class
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            processor.Map(source, destination, new MappingContext());
        }

        public static TDestination Translate<TDestination>(this IMappingProcessor processor,
                                                           object source)
            where TDestination : class
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.Translate<TDestination>(source, new MappingContext());
        }

        public static TDestination Translate<TSource, TDestination>(this IMappingProcessor processor,
                                                                    TSource source)
            where TSource : class
            where TDestination : class
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.Translate<TSource, TDestination>(source, new MappingContext());
        }

        public static void Map<TSource, TDestination>(this IMappingProcessor processor,
                                                      TSource source,
                                                      TDestination destination,
                                                      object context)
            where TSource : class
            where TDestination : class
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            processor.Map(source, destination, MappingContext.FromObject(context));
        }

        public static TDestination Translate<TDestination>(this IMappingProcessor processor,
                                                           object source,
                                                           object context)
            where TDestination : class
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.Translate<TDestination>(source, MappingContext.FromObject(context));
        }

        public static TDestination Translate<TSource, TDestination>(this IMappingProcessor processor,
                                                                    TSource source,
                                                                    object context)
            where TSource : class
            where TDestination : class
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.Translate<TSource, TDestination>(source, MappingContext.FromObject(context));
        }

        public static IEnumerable<TDestination> TranslateCollection<TSource, TDestination>(this IMappingProcessor processor,
                                                                                           IEnumerable<TSource> source)
            where TSource : class
            where TDestination : class
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.TranslateCollection<TSource, TDestination>(source, new MappingContext());
        }

        public static IEnumerable<TDestination> TranslateCollection<TDestination>(this IMappingProcessor processor,
                                                                                  IEnumerable<object> source)
            where TDestination : class
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.TranslateCollection<TDestination>(source, new MappingContext());
        }

        public static IEnumerable<TDestination> TranslateCollection<TSource, TDestination>(this IMappingProcessor processor,
                                                                                           IEnumerable<TSource> source,
                                                                                           IMappingContext context)
            where TSource : class
            where TDestination : class
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            Ensure.That(context, nameof(context)).IsNotNull();
            foreach (var item in source)
                yield return processor.Translate<TSource, TDestination>(item, context);
        }

        public static IEnumerable<TDestination> TranslateCollection<TDestination>(this IMappingProcessor processor,
                                                                                  IEnumerable<object> source,
                                                                                  IMappingContext context)
            where TDestination : class
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            Ensure.That(context, nameof(context)).IsNotNull();
            foreach (var item in source)
                yield return processor.Translate<TDestination>(item, context);
        }

        public static IEnumerable<TDestination> TranslateCollection<TSource, TDestination>(this IMappingProcessor processor,
                                                                                           IEnumerable<TSource> source,
                                                                                           object context)
            where TSource : class
            where TDestination : class
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.TranslateCollection<TSource, TDestination>(source, MappingContext.FromObject(context));
        }

        public static IEnumerable<TDestination> TranslateCollection<TDestination>(this IMappingProcessor processor,
                                                                                  IEnumerable<object> source,
                                                                                  object context)
            where TDestination : class
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.TranslateCollection<TDestination>(source, MappingContext.FromObject(context));
        }

        #endregion Methods

    }
}
