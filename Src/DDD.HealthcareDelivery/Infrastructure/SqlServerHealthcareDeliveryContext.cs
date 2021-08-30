using Microsoft.EntityFrameworkCore;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Infrastructure.Data;
    using Prescriptions;

    public class SqlServerHealthcareDeliveryContext : HealthcareDeliveryContext
    {

        #region Constructors

        public SqlServerHealthcareDeliveryContext(string connectionString) : base(connectionString)
        {
        }

        #endregion Constructors

        #region Methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.ConnectionString);
        }

        protected override void ApplyConfigurations(ModelBuilder modelBuilder)
        {
            base.ApplyConfigurations(modelBuilder);
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.ApplyConfiguration(new SqlServerStoredEventConfiguration());
            modelBuilder.ApplyConfiguration(new SqlServerPrescriptionStateConfiguration());
            modelBuilder.ApplyConfiguration(new SqlServerPrescribedMedicationStateConfiguration());
        }

        #endregion Methods

    }
}
