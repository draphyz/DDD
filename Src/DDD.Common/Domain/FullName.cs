using Conditions;
using System;
using System.Collections.Generic;

namespace DDD.Common.Domain
{
    using Core.Domain;

    public class FullName : ComparableValueObject, IStateObjectConvertible<FullNameState>
    {

        #region Constructors

        public FullName(string lastName, string firstName)
        {
            Condition.Requires(lastName, nameof(lastName)).IsNotNullOrWhiteSpace();
            Condition.Requires(firstName, nameof(firstName)).IsNotNullOrWhiteSpace();
            this.LastName = lastName.ToTitleCase();
            this.FirstName = firstName.ToTitleCase();
        }

        #endregion Constructors 

        #region Properties

        public string FirstName { get; }

        public string LastName { get; }

        #endregion Properties

        #region Methods

        public static FullName FromState(FullNameState state)
        {
            if (state == null) return null;
            return new FullName
            (
                state.LastName,
                state.FirstName
            );
        }

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

        public FullNameState ToState()
        {
            return new FullNameState
            {
                LastName = this.LastName,
                FirstName = this.FirstName
            };
        }

        public override string ToString()
        {
            return $"{this.GetType().Name} [lastName={this.LastName}, firstName={this.FirstName}]";
        }

        public FullName WithFirstName(string firstName)
        {
            Condition.Requires(firstName, nameof(firstName)).IsNotNullOrWhiteSpace();
            return new FullName(this.LastName, firstName);
        }

        public FullName WithLastName(string lastName)
        {
            Condition.Requires(lastName, nameof(lastName)).IsNotNullOrWhiteSpace();
            return new FullName(lastName, this.FirstName);
        }

        #endregion Methods
    }
}