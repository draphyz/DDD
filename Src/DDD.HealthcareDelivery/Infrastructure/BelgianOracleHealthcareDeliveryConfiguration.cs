using NHibernate.Mapping.ByCode;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Prescriptions;
    using Domain.Prescriptions;
    using Domain.Practitioners;
    using Common.Domain;

    public class BelgianOracleHealthcareDeliveryConfiguration : OracleHealthcareDeliveryConfiguration
    {

        #region Methods

        protected override void AddMappings(ModelMapper modelMapper)
        {
            base.AddMappings(modelMapper);
            modelMapper.AddMapping<OraclePrescriptionMapping<BelgianHealthcarePractitionerLicenseNumber,
                                                             BelgianSocialSecurityNumber,
                                                             BelgianSex>>();
            modelMapper.AddMapping<OraclePrescribedMedicationMapping<BelgianMedicationCode>>();
        }

        #endregion Methods

    }
}
