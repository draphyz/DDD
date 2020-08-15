using NHibernate.Mapping.ByCode;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Prescriptions;
    using Domain.Prescriptions;
    using Domain.Practitioners;
    using Domain.Facilities;
    using Common.Domain;

    public class BelgianSqlServerHealthcareDeliveryConfiguration : SqlServerHealthcareDeliveryConfiguration
    {

        #region Constructors

        public BelgianSqlServerHealthcareDeliveryConfiguration(string connectionString) : base(connectionString)
        {
        }

        #endregion Constructors

        #region Methods

        protected override void AddMappings(ModelMapper modelMapper)
        {
            base.AddMappings(modelMapper);
            modelMapper.AddMapping<SqlServerPrescriptionMapping<BelgianHealthcarePractitionerLicenseNumber,
                                                                BelgianHealthFacilityLicenseNumber,
                                                                BelgianSocialSecurityNumber,
                                                                BelgianSex>>();
            modelMapper.AddMapping<SqlServerPrescribedMedicationMapping<BelgianMedicationCode>>();
        }

        #endregion Methods

    }
}
