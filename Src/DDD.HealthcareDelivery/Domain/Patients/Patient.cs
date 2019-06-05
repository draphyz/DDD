using Conditions;
using System;
using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Domain.Patients
{
    using Common.Domain;
    using Core.Domain;

    public class Patient : ValueObject
    {

        #region Constructors

        public Patient(int identifier,
                       FullName fullName,
                       Sex sex,
                       SocialSecurityNumber socialSecurityNumber = null,
                       ContactInformation contactInformation = null,
                       DateTime? birthdate = null)
        {
            Condition.Requires(identifier, nameof(identifier)).IsGreaterThan(0);
            Condition.Requires(fullName, nameof(fullName)).IsNotNull();
            Condition.Requires(sex, nameof(sex)).IsNotNull();
            this.Identifier = identifier;
            this.FullName = fullName;
            this.Sex = sex;
            this.SocialSecurityNumber = socialSecurityNumber;
            this.ContactInformation = contactInformation;
            this.Birthdate = birthdate;
        }

        protected Patient() { }

        #endregion Constructors

        #region Properties

        public DateTime? Birthdate { get; private set; }

        public ContactInformation ContactInformation { get; private set; }

        public FullName FullName { get; private set; }

        public int Identifier { get; private set; }

        public Sex Sex { get; private set; }

        public SocialSecurityNumber SocialSecurityNumber { get; private set; }

        #endregion Properties

        #region Methods

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.Identifier;
            yield return this.FullName;
            yield return this.Sex;
            yield return this.SocialSecurityNumber;
            yield return this.ContactInformation;
            yield return this.Birthdate;
        }

        public override string ToString()
        {
            var format = "{0} [identifier={1}, fullName={2}, sex={3}, socialSecurityNumber={4}, contactInformation={5}, birthdate={6}]";
            return string.Format(format, this.GetType().Name, this.Identifier, this.FullName, this.Sex, this.SocialSecurityNumber, this.ContactInformation, this.Birthdate);
        }

        #endregion Methods

    }
}