using Conditions;
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

        protected IdentificationCode(string code)
        {
            Condition.Requires(code, nameof(code)).IsNotNullOrWhiteSpace();
            this.Code = code.ToUpper();
        }

        #endregion Constructors

        #region Properties

        public string Code { get; }

        #endregion Properties

        #region Methods

        public override IEnumerable<IComparable> ComparableComponents()
        {
            yield return this.Code;
        }

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.Code;
        }

        public override string ToString() => $"{this.GetType().Name} [code={this.Code}]";

        #endregion Methods

    }
}
