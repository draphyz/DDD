using System;
using System.Collections.Generic;
using Conditions;

namespace DDD.Mapping
{
    public class DelegatingTranslator<TSource, TDestination> : IObjectTranslator<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {

        #region Fields

        private readonly Func<TSource, IDictionary<string, object>, TDestination> toDestination;

        #endregion Fields

        #region Constructors

        public DelegatingTranslator(Func<TSource, IDictionary<string, object>, TDestination> toDestination)
        {
            Condition.Requires(toDestination).IsNotNull();
            this.toDestination = toDestination;
        }

        #endregion Constructors

        #region Methods

        public TDestination Translate(TSource source, IDictionary<string, object> options = null)
        {
            Condition.Requires(source).IsNotNull();
            return this.toDestination(source, options);
        }

        #endregion Methods

    }
}
