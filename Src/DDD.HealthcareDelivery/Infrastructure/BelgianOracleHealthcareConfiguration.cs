using NHibernate.Mapping.ByCode;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Prescriptions;
    using Domain.Prescriptions;
    using Domain.Practitioners;
    using Domain.Facilities;
    using Common.Domain;

    public class BelgianOracleHealthcareConfiguration : OracleHealthcareConfiguration
    {

        #region Constructors

        public BelgianOracleHealthcareConfiguration(string connectionString) : base(connectionString)
        {
        }

        #endregion Constructors

        #region Methods

        protected override void InitializeModel(ModelMapper modelMapper)
        {
            base.InitializeModel(modelMapper);
            modelMapper.AddMapping<OraclePrescriptionMapping<BelgianHealthcarePractitionerLicenseNumber,
                                                             BelgianHealthFacilityLicenseNumber,
                                                             BelgianSocialSecurityNumber,
                                                             BelgianSex>>();
            modelMapper.AddMapping<OraclePrescribedMedicationMapping<BelgianMedicationCode>>();
        }

        #endregion Methods

    }
}
