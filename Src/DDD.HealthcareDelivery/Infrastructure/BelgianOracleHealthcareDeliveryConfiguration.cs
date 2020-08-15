using NHibernate.Mapping.ByCode;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Prescriptions;
    using Domain.Prescriptions;
    using Domain.Practitioners;
    using Domain.Facilities;
    using Common.Domain;

    public class BelgianOracleHealthcareDeliveryConfiguration : OracleHealthcareDeliveryConfiguration
    {

        #region Constructors

        public BelgianOracleHealthcareDeliveryConfiguration(string connectionString) : base(connectionString)
        {
        }

        #endregion Constructors

        #region Methods

        protected override void AddMappings(ModelMapper modelMapper)
        {
            base.AddMappings(modelMapper);
            modelMapper.AddMapping<OraclePrescriptionMapping<BelgianHealthcarePractitionerLicenseNumber,
                                                             BelgianHealthFacilityLicenseNumber,
                                                             BelgianSocialSecurityNumber,
                                                             BelgianSex>>();
            modelMapper.AddMapping<OraclePrescribedMedicationMapping<BelgianMedicationCode>>();
        }

        #endregion Methods

    }
}
