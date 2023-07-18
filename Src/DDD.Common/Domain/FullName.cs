using EnsureThat;
using System;
using System.Collections.Generic;

namespace DDD.Common.Domain
{
    using Core.Domain;

    public class FullName : ComparableValueObject
    {

        #region Constructors

        public FullName(string lastName, string firstName)
        {
            Ensure.That(lastName, nameof(lastName)).IsNotNullOrWhiteSpace();
            Ensure.That(firstName, nameof(firstName)).IsNotNullOrWhiteSpace();
            this.LastName = lastName.ToTitleCase();
            this.FirstName = firstName.ToTitleCase();
        }

        protected FullName() { }

        #endregion Constructors

        #region Properties

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        #endregion Properties

        #region Methods

        public string AsFormattedName() => $"{this.LastName.ToUpper()} {this.FirstName}";

        public override IEnumerable<IComparable> ComparableComponents()
        {
            yield return this.LastName;
            yield return this.FirstName;
        }

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.LastName;
            yield return this.FirstName;
        }

        public override string ToString()
        {
            return $"{GetType().Name} [{nameof(LastName)}={LastName}, {nameof(FirstName)}={FirstName}]";
        }

        public FullName WithFirstName(string firstName)
        {
            Ensure.That(firstName, nameof(firstName)).IsNotNullOrWhiteSpace();
            return new FullName(this.LastName, firstName);
        }

        public FullName WithLastName(string lastName)
        {
            Ensure.That(lastName, nameof(lastName)).IsNotNullOrWhiteSpace();
            return new FullName(lastName, this.FirstName);
        }

        #endregion Methods

    }
}