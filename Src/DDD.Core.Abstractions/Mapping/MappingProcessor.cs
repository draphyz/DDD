using Conditions;
using System;
using System.Collections.Generic;

namespace DDD.Mapping
{
    /// <summary>
    /// Finds the correct mapper or translator and invokes it.  
    /// </summary>
    public class MappingProcessor : IMappingProcessor
    {

        #region Fields

        private readonly IServiceProvider serviceProvider;

        #endregion Fields

        #region Constructors

        public MappingProcessor(IServiceProvider serviceProvider)
        {
            Condition.Requires(serviceProvider, nameof(serviceProvider)).IsNotNull();
            this.serviceProvider = serviceProvider;
        }

        #endregion Constructors

        #region Methods

        public void Map<TSource, TDestination>(TSource source, TDestination destination, IDictionary<string, object> options = null)
            where TSource : class
            where TDestination : class
        {
            var mapper = this.serviceProvider.GetService<IObjectMapper<TSource, TDestination>>();
            if (mapper == null) throw new InvalidOperationException($"The mapper for type {typeof(IObjectMapper<TSource, TDestination>)} could not be found.");
            mapper.Map(source, destination, options);
        }

        public TDestination Translate<TDestination>(object source, IDictionary<string, object> options = null)
            where TDestination : class
        {
            if (source == null) return null;
            var translatorType = typeof(IObjectTranslator<,>).MakeGenericType(source.GetType(), typeof(TDestination));
            dynamic translator = this.serviceProvider.GetService(translatorType);
            if (translator == null) throw new InvalidOperationException($"The translator for type {translatorType} could not be found.");
            return translator.Translate((dynamic)source, options);
        }

        public TDestination Translate<TSource, TDestination>(TSource source, IDictionary<string, object> options = null)
            where TSource : class
            where TDestination : class
        {
            var translator = this.serviceProvider.GetService<IObjectTranslator<TSource, TDestination>>();
            if (translator == null) throw new InvalidOperationException($"The translator for type {typeof(IObjectTranslator<TSource, TDestination>)} could not be found.");
            return translator.Translate(source, options);
        }

        #endregion Methods

    }
}
