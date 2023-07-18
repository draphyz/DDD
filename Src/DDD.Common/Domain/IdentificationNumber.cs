using EnsureThat;
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

        protected IdentificationNumber() { }

        protected IdentificationNumber(string value)
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

        public override string ToString() => $"{GetType().Name} [{nameof(Value)}={Value}]";

        #endregion Methods

    }
}
