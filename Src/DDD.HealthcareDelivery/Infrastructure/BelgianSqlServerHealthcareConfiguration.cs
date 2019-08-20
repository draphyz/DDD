using NHibernate.Mapping.ByCode;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Prescriptions;
    using Domain.Prescriptions;
    using Domain.Practitioners;
    using Domain.Facilities;
    using Common.Domain;

    public class BelgianSqlServerHealthcareConfiguration : SqlServerHealthcareConfiguration
    {

        #region Constructors

        public BelgianSqlServerHealthcareConfiguration(string connectionString) : base(connectionString)
        {
        }

        #endregion Constructors

        #region Methods

        protected override void InitializeModel(ModelMapper modelMapper)
        {
            base.InitializeModel(modelMapper);
            modelMapper.AddMapping<SqlServerPrescriptionMapping<BelgianHealthcarePractitionerLicenseNumber,
                                                                BelgianHealthFacilityLicenseNumber,
                                                                BelgianSocialSecurityNumber,
                                                                BelgianSex>>();
            modelMapper.AddMapping<SqlServerPrescribedMedicationMapping<BelgianMedicationCode>>();
        }

        #endregion Methods

    }
}
