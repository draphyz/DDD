using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Prescriptions;
    using Core.Infrastructure.Data;

    public class OracleHealthcareDeliveryContext : HealthcareDeliveryContext
    {
        

        #region Constructors

        public OracleHealthcareDeliveryContext(IHealthcareDeliveryConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        #endregion Constructors

        #region Methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle(this.Connection, o => o.UseOracleSQLCompatibility("11"));
        }

        protected override void ApplyConfigurations(ModelBuilder modelBuilder)
        {
            base.ApplyConfigurations(modelBuilder);
            var connectionBuilder = new OracleConnectionStringBuilder(this.Connection.ConnectionString);
            modelBuilder.HasDefaultSchema(connectionBuilder.UserID);
            modelBuilder.ApplyConfiguration(new OracleEventStateConfiguration());
            modelBuilder.ApplyConfiguration(new OraclePrescriptionStateConfiguration());
            modelBuilder.ApplyConfiguration(new OraclePrescribedMedicationStateConfiguration());
        }

        protected override void ApplyConventions(ModelBuilder modelBuilder)
        {
            base.ApplyConventions(modelBuilder);
            modelBuilder.ApplyUpperCaseNamingConvention();
        }

        #endregion Methods

    }
}
