using NHibernate.Mapping.ByCode;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Prescriptions;
    using Domain.Prescriptions;
    using Domain.Practitioners;
    using Common.Domain;

    public class BelgianSqlServerHealthcareDeliveryConfiguration : SqlServerHealthcareDeliveryConfiguration
    {

        #region Methods

        protected override void AddMappings(ModelMapper modelMapper)
        {
            base.AddMappings(modelMapper);
            modelMapper.AddMapping<SqlServerPrescriptionMapping<BelgianHealthcarePractitionerLicenseNumber,
                                                                BelgianSocialSecurityNumber,
                                                                BelgianSex>>();
            modelMapper.AddMapping<SqlServerPrescribedMedicationMapping<BelgianMedicationCode>>();
        }

        #endregion Methods

    }
}
