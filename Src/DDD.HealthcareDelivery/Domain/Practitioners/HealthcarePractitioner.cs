using System.Collections.Generic;
using Conditions;

namespace DDD.HealthcareDelivery.Domain.Practitioners
{
    using Common.Domain;
    using Core.Domain;

    /// <summary>
    /// Represents a person that provides healthcare services.
    /// </summary>
    public abstract class HealthcarePractitioner 
        : ValueObject, IStateObjectConvertible<HealthcarePractitionerState>
    {

        #region Constructors

        protected HealthcarePractitioner(int identifier,
                                         FullName fullName,
                                         HealthcarePractitionerLicenseNumber licenseNumber,
                                         SocialSecurityNumber socialSecurityNumber = null,
                                         ContactInformation contactInformation = null,
                                         string speciality = null,
                                         string displayName = null)
        {
            Condition.Requires(identifier, nameof(identifier)).IsGreaterThan(0);
            Condition.Requires(fullName, nameof(fullName)).IsNotNull();
            Condition.Requires(licenseNumber, nameof(licenseNumber)).IsNotNull();
            this.Identifier = identifier;
            this.FullName = fullName;
            this.LicenseNumber = licenseNumber;
            this.SocialSecurityNumber = socialSecurityNumber;
            this.ContactInformation = contactInformation;
            if (!string.IsNullOrWhiteSpace(speciality))
                this.Speciality = speciality;
            this.DisplayName = string.IsNullOrWhiteSpace(displayName) ? fullName.AsFormattedName() : displayName;
        }

        #endregion Constructors

        #region Properties

        public ContactInformation ContactInformation { get; }

        public FullName FullName { get; }

        public string DisplayName { get; }

        public int Identifier { get; }

        public HealthcarePractitionerLicenseNumber LicenseNumber { get; }

        public SocialSecurityNumber SocialSecurityNumber { get; }

        public string Speciality { get; }

        #endregion Properties

        #region Methods

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.Identifier;
            yield return this.FullName;
            yield return this.LicenseNumber;
            yield return this.SocialSecurityNumber;
            yield return this.ContactInformation;
            yield return this.Speciality;
            yield return this.DisplayName;
        }

        public virtual HealthcarePractitionerState ToState()
        {
            return new HealthcarePractitionerState
            {
                Identifier = this.Identifier,
                FullName = this.FullName.ToState(),
                LicenseNumber = this.LicenseNumber.Value, 
                SocialSecurityNumber = this.SocialSecurityNumber?.Value,
                ContactInformation = this.ContactInformation?.ToState(), 
                Speciality = this.Speciality,
                DisplayName = this.DisplayName
            };
        }

        public override string ToString()
        {
            var format = "{0} [identifier={1}, fullName={2}, licenseNumber={3}, socialSecurityNumber={4}, contactInformation={5}, speciality={6}, displayName={7}]";
            return string.Format(format, this.GetType().Name, this.Identifier, this.FullName, this.LicenseNumber, this.SocialSecurityNumber, this.ContactInformation, this.Speciality, this.DisplayName);
        }

        #endregion Methods

    }
}