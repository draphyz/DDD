using EnsureThat;
using System;

namespace DDD.Mapping
{
    /// <summary>
    /// Finds the correct mapper or translator and invokes it.  
    /// </summary>
    public class MappingProcessor : IMappingProcessor
    {

        #region Fields

        private readonly IServiceProvider serviceProvider;
        private readonly MappingProcessorSettings settings;

        #endregion Fields

        #region Constructors

        public MappingProcessor(IServiceProvider serviceProvider, MappingProcessorSettings settings)
        {
            Ensure.That(serviceProvider, nameof(serviceProvider)).IsNotNull();
            Ensure.That(settings, nameof(settings)).IsNotNull();
            this.serviceProvider = serviceProvider;
            this.settings = settings;
        }

        #endregion Constructors

        #region Methods

        public void Map<TSource, TDestination>(TSource source, TDestination destination, IMappingContext context)
            where TSource : class
            where TDestination : class
        {
            var mapper = this.serviceProvider.GetService<IObjectMapper<TSource, TDestination>>();
            if (mapper == null)
            {
                if (this.settings.DefaultMapper == null)
                    throw new InvalidOperationException($"The mapper for type {typeof(IObjectMapper<TSource, TDestination>)} could not be found.");
                this.settings.DefaultMapper.Map(source, destination);
            }
            else
                mapper.Map(source, destination, context);
        }

        public TDestination Translate<TDestination>(object source, IMappingContext context)
            where TDestination : class
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            if (source == null) return null;
            var translatorType = typeof(IObjectTranslator<,>).MakeGenericType(source.GetType(), typeof(TDestination));
            dynamic translator = this.serviceProvider.GetService(translatorType);
            if (translator == null)
            {
                if (this.settings.DefaultTranslator == null)
                    throw new InvalidOperationException($"The translator for type {translatorType} could not be found.");
                context.AddDestinationType(typeof(TDestination));
                return (TDestination)this.settings.DefaultTranslator.Translate(source, context);
            }
            return translator.Translate((dynamic)source, context);
        }

        public TDestination Translate<TSource, TDestination>(TSource source, IMappingContext context)
            where TSource : class
            where TDestination : class
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            var translator = this.serviceProvider.GetService<IObjectTranslator<TSource, TDestination>>();
            if (translator == null)
            {
                if (this.settings.DefaultTranslator == null)
                    throw new InvalidOperationException($"The translator for type {typeof(IObjectTranslator<TSource, TDestination>)} could not be found.");
                context.AddDestinationType(typeof(TDestination));
                return (TDestination)this.settings.DefaultTranslator.Translate(source, context);
            }
            return translator.Translate(source, context);
        }

        #endregion Methods

    }
}
