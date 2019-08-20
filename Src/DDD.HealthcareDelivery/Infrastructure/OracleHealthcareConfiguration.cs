using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Prescriptions;
    using Core.Infrastructure.Data;

    public abstract class OracleHealthcareConfiguration : HealthcareConfiguration
    {

        #region Constructors

        protected OracleHealthcareConfiguration(string connectionString) : base(connectionString)
        {
            this.DataBaseIntegration(db =>
            {
                db.Dialect<Oracle10gDialect>();
                db.Driver<OracleManagedDataClientDriver>();
            });
            this.SetNamingStrategy(UpperCaseNamingStrategy.Instance);
        }

        #endregion Constructors

        #region Methods

        protected override void InitializeModel(ModelMapper modelMapper)
        {
            base.InitializeModel(modelMapper);
            modelMapper.AddMapping<OraclePharmaceuticalPrescriptionMapping>();
            modelMapper.AddMapping<OracleStoredEventMapping>();
        }

        #endregion Methods

    }
}
