using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Infrastructure.Data;

    public abstract class SqlServerHealthcareDeliveryConfiguration : HealthcareDeliveryConfiguration
    {

        #region Constructors

        protected SqlServerHealthcareDeliveryConfiguration(string connectionString) : base(connectionString)
        {
            this.DataBaseIntegration(db =>
            {
                db.Dialect<MsSql2012Dialect>();
                db.Driver<MicrosoftDataSqlClientDriver>();
            });
        }

        #endregion Constructors

        #region Methods

        protected override void AddMappings(ModelMapper modelMapper)
        {
            base.AddMappings(modelMapper);
            modelMapper.AddMapping<SqlServerStoredEventMapping>();
        }

        #endregion Methods
    }
}
