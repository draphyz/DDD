﻿using System;
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

        protected ArbitraryIdentifier() { }

        protected ArbitraryIdentifier(TId value)
        {
            this.Value = value;
        }

        #endregion Constructors

        #region Properties

        public TId Value { get; private set; }

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

        public override string ToString() => $"{GetType().Name} [{nameof(Value)}={Value}]";

        #endregion Methods

    }
}
