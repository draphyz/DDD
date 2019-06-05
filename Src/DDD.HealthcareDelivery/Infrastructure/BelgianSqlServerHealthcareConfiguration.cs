using NHibernate.Mapping.ByCode;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Prescriptions;

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
            modelMapper.AddMapping<BelgianSqlServerPrescriptionMapping>();
        }

        #endregion Methods

    }
}
