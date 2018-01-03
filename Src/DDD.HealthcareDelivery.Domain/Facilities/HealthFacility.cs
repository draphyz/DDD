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
                                 HealthFacilityLicenseNumber licenseNumber = null, 
                                 string code = null)
        {
            Condition.Requires(identifier, nameof(identifier)).IsGreaterThan(0);
            Condition.Requires(name, nameof(name)).IsNotNullOrWhiteSpace();
            this.Identifier = identifier;
            this.Name = name;
            this.LicenseNumber = licenseNumber;
            if (!string.IsNullOrWhiteSpace(code))
                this.Code = code.ToUpper();
        }

        #endregion Constructors

        #region Properties

        public int Identifier { get; }

        public string Code { get; }

        public HealthFacilityLicenseNumber LicenseNumber { get; }

        public string Name { get; }

        #endregion Properties

        #region Methods

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.Name;
            yield return this.LicenseNumber;
            yield return this.Code;
        }

        public virtual HealthFacilityState ToState()
        {
            return new HealthFacilityState
            {
                Identifier = this.Identifier,
                Name = this.Name,
                LicenseNumber = this.LicenseNumber?.Number,
                Code = this.Code,
            };
        }

        public override string ToString()
        {
            return $"{this.GetType().Name} [name={this.Name}, licenseNumber={this.LicenseNumber}, code={this.Code}]";
        }

        #endregion Methods

    }
}
