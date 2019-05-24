using Conditions;
using System;
using System.Collections.Generic;

namespace DDD.Common.Domain
{
    using Core.Domain;

    /// <summary>
    /// Represents an identifier that has a symbolic meaning.
    /// </summary>
    public abstract class IdentificationNumber : ComparableValueObject
    {

        #region Constructors

        protected IdentificationNumber(string value)
        {
            Condition.Requires(value, nameof(value)).IsNotNullOrWhiteSpace();
            this.Value = value.ToUpper();
        }

        #endregion Constructors

        #region Properties

        public string Value { get; }

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
