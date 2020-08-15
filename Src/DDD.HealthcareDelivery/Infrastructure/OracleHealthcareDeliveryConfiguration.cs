using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Infrastructure.Data;

    public abstract class OracleHealthcareDeliveryConfiguration : HealthcareDeliveryConfiguration
    {

        #region Constructors

        protected OracleHealthcareDeliveryConfiguration(string connectionString) : base(connectionString)
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

        protected override void AddMappings(ModelMapper modelMapper)
        {
            base.AddMappings(modelMapper);
            modelMapper.AddMapping<OracleStoredEventMapping>();
        }

        #endregion Methods

    }
}
