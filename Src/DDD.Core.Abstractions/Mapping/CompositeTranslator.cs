using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DDD.Mapping
{
    /// <summary>
    /// A translator composed from multiple child translators.
    /// </summary>
    public class CompositeTranslator<TSource, TDestination> : ObjectTranslator<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {

        #region Fields

        private readonly List<IObjectTranslator> translators = new List<IObjectTranslator>();

        #endregion Fields

        #region Methods

        /// <summary>
        /// Registers a child translator.
        /// </summary>
        /// <remarks>
        /// The order of registrations is important. Register the translators from the most derived source type to the least derived source type.
        /// </remarks>
        public void Register<TDerivedSource>(IObjectTranslator<TDerivedSource, TDestination> translator)
            where TDerivedSource : class, TSource
        {
            Ensure.That(translator, nameof(translator)).IsNotNull();
            this.translators.Add(translator);
        }

        /// <summary>
        /// Registers a child translator from a delegate.
        /// </summary>
        /// <remarks>
        /// The order of registrations is important. Register the translators from the most derived source type to the least derived source type.
        /// </remarks>
        public void Register<TDerivedSource>(Func<TDerivedSource, IMappingContext, TDestination> translator)
            where TDerivedSource : class, TSource
        {
            Ensure.That(translator, nameof(translator)).IsNotNull();
            this.translators.Add(new DelegatingTranslator<TDerivedSource, TDestination>(translator));
        }

        public override TDestination Translate(TSource source, IMappingContext context)
        {
            if (source == null) return null;
            var translator = this.translators.FirstOrDefault(t => t.SourceType.IsAssignableFrom(source.GetType()));
            if (translator != null)
                return (TDestination)translator.Translate(source, context);
            throw new MappingException($"No child translator registered for '{source.GetType()}'.");
        }

        #endregion Methods

    }
}
