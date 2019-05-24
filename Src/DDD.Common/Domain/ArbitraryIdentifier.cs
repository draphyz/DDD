using System;
using System.Collections.Generic;

namespace DDD.Common.Domain
{
    using Core.Domain;

    /// <summary>
    /// Represents an identifier that has no symbolic meaning. 
    /// </summary>
    public abstract class ArbitraryIdentifier<TId> : ComparableValueObject
        where TId : IComparable
    {

        #region Constructors

        protected ArbitraryIdentifier(TId value)
        {
            this.Value = value;
        }

        #endregion Constructors

        #region Properties

        public TId Value { get; }

        #endregion Properties

        #region Methods

        public override IEnumerable<IComparable> ComparableComponents()
        {
            yield return this.Value;
        }

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.Value;
        }

        public override string ToString() => $"{this.GetType().Name} [value={this.Value}]";

        #endregion Methods

    }
}
