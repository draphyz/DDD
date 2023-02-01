using EnsureThat;
using System;
using System.Collections.Generic;

namespace DDD.Common.Domain
{
    using Core.Domain;

    /// <summary>
    /// Represents an identifier that follows an encoding system.
    /// </summary>
    public abstract class IdentificationCode : ComparableValueObject
    {

        #region Constructors

        protected IdentificationCode() { }

        protected IdentificationCode(string value)
        {
            Ensure.That(value, nameof(value)).IsNotNullOrWhiteSpace();
            this.Value = value.ToUpper();
        }

        #endregion Constructors

        #region Properties

        public string Value { get; private set; }

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
