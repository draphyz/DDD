using System;
using EnsureThat;

namespace DDD.Mapping
{
    /// <summary>
    /// Adapter that converts a delegate into an object that implements the interface IObjectTranslator.
    /// </summary>
    public class DelegatingTranslator<TSource, TDestination> : ObjectTranslator<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {

        #region Fields

        private readonly Func<TSource, IMappingContext, TDestination> translator;

        #endregion Fields

        #region Constructors

        public DelegatingTranslator(Func<TSource, IMappingContext, TDestination> translator)
        {
            Ensure.That(translator).IsNotNull();
            this.translator = translator;
        }

        #endregion Constructors

        #region Methods

        public override TDestination Translate(TSource source, IMappingContext context)
        {
            Ensure.That(source).IsNotNull();
            Ensure.That(context, nameof(context)).IsNotNull();
            return this.translator(source, context);
        }

        #endregion Methods

    }
}
