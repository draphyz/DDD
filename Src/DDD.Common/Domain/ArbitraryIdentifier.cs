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

        protected ArbitraryIdentifier(TId identifier)
        {
            this.Identifier = identifier;
        }

        #endregion Constructors

        #region Properties

        public TId Identifier { get; }

        #endregion Properties

        #region Methods

        public override IEnumerable<IComparable> ComparableComponents()
        {
            yield return this.Identifier;
        }

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.Identifier;
        }

        public override string ToString() => $"{this.GetType().Name} [identifier={this.Identifier}]";

        #endregion Methods

    }
}
