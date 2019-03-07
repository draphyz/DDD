using System.Collections.Generic;
using Conditions;

namespace DDD.HealthcareDelivery.Domain.Facilities
{
    using Core.Domain;

    /// <summary>
    /// Represents any location where healthcare is provided.
    /// </summary>
    public abstract class HealthFacility : ValueObject, IStateObjectConvertible<HealthFacilityState>
    {

        #region Constructors

        protected HealthFacility(int identifier, 
                                 string name, 
                                 HealthFacilityLicenseNumber licenseNumber = null)
        {
            Condition.Requires(identifier, nameof(identifier)).IsGreaterThan(0);
            Condition.Requires(name, nameof(name)).IsNotNullOrWhiteSpace();
            this.Identifier = identifier;
            this.Name = name;
            this.LicenseNumber = licenseNumber;
        }

        #endregion Constructors

        #region Properties

        public int Identifier { get; }

        public HealthFacilityLicenseNumber LicenseNumber { get; }

        public string Name { get; }

        #endregion Properties

        #region Methods

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.Identifier;
            yield return this.Name;
            yield return this.LicenseNumber;
        }

        public virtual HealthFacilityState ToState()
        {
            return new HealthFacilityState
            {
                Identifier = this.Identifier,
                Name = this.Name,
                LicenseNumber = this.LicenseNumber?.Number
            };
        }

        public override string ToString()
        {
            return $"{this.GetType().Name} [identifier={this.Identifier}, name={this.Name}, licenseNumber={this.LicenseNumber}]";
        }

        #endregion Methods

    }
}
