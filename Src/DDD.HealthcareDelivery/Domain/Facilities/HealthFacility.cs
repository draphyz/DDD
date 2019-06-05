using Conditions;
using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Domain.Facilities
{
    using Core.Domain;

    /// <summary>
    /// Represents any location where healthcare is provided.
    /// </summary>
    public abstract class HealthFacility : ValueObject
    {

        #region Constructors

        protected HealthFacility() { }

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

        public int Identifier { get; private set; }

        public HealthFacilityLicenseNumber LicenseNumber { get; private set; }

        public string Name { get; private set; }

        #endregion Properties

        #region Methods

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.Identifier;
            yield return this.Name;
            yield return this.LicenseNumber;
        }

        public override string ToString()
        {
            return $"{this.GetType().Name} [identifier={this.Identifier}, name={this.Name}, licenseNumber={this.LicenseNumber}]";
        }

        #endregion Methods

    }
}
