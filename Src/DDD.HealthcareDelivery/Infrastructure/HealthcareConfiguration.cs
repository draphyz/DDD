using Conditions;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Prescriptions;

    public abstract class HealthcareConfiguration : Configuration
    {

        #region Constructors

        protected HealthcareConfiguration(string connectionString)
        {
            Condition.Requires(connectionString, nameof(connectionString)).IsNotNullOrWhiteSpace();
            var modelMapper = new ModelMapper();
            this.InitializeModel(modelMapper);
            this.DataBaseIntegration(db =>
            {
                db.ConnectionString = connectionString;
                db.ConnectionReleaseMode = ConnectionReleaseMode.OnClose;
                db.LogSqlInConsole = true;
                db.LogFormattedSql = true;
            });
            this.AddMapping(modelMapper.CompileMappingForAllExplicitlyAddedEntities());
        }

        #endregion Constructors

        #region Methods

        protected virtual void InitializeModel(ModelMapper modelMapper)
        {
            modelMapper.AddMapping<PrescribedPharmaceuticalProductMapping>();
        }

        #endregion Methods

    }
}
