using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Prescriptions;

    public abstract class SqlServerHealthcareConfiguration : HealthcareConfiguration
    {

        #region Constructors

        protected SqlServerHealthcareConfiguration(string connectionString) : base(connectionString)
        {
            this.DataBaseIntegration(db =>
            {
                db.Dialect<MsSql2012Dialect>();
                db.Driver<SqlClientDriver>();
            });
        }

        #endregion Constructors

        #region Methods

        protected override void InitializeModel(ModelMapper modelMapper)
        {
            base.InitializeModel(modelMapper);
            modelMapper.AddMapping<SqlServerPharmaceuticalPrescriptionMapping>();
            modelMapper.AddMapping<SqlServerStoredEventMapping>();
        }

        #endregion Methods
    }
}
