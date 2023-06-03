using System;

namespace DDD.Mapping
{
    /// <summary>
    /// Base class for translating objects.
    /// </summary>
    public abstract class ObjectTranslator<TSource, TDestination> : IObjectTranslator<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        #region Properties

        Type IObjectTranslator.SourceType => typeof(TSource);

        Type IObjectTranslator.DestinationType => typeof(TDestination);

        #endregion Properties

        #region Methods

        public abstract TDestination Translate(TSource source, IMappingContext context);

        object IObjectTranslator.Translate(object source, IMappingContext context) => this.Translate((TSource)source, context);

        #endregion Methods
    }
}
