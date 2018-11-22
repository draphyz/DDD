using System;
using Conditions;

namespace DDD.Mapping
{
    public class DelegatingTranslator<TSource, TDestination> : IObjectTranslator<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {

        #region Fields

        private readonly Func<TSource, TDestination> toDestination;

        #endregion Fields

        #region Constructors

        public DelegatingTranslator(Func<TSource, TDestination> toDestination)
        {
            Condition.Requires(toDestination).IsNotNull();
            this.toDestination = toDestination;
        }

        #endregion Constructors

        #region Methods

        public TDestination Translate(TSource source)
        {
            Condition.Requires(source).IsNotNull();
            return this.toDestination(source);
        }

        #endregion Methods

    }
}
