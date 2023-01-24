using Microsoft.EntityFrameworkCore;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Domain;
    using Core.Infrastructure.Data;
    using Prescriptions;

    public class SqlServerHealthcareDeliveryContext : DbHealthcareDeliveryContext
    {

        #region Constructors

        public SqlServerHealthcareDeliveryContext(IDbConnectionProvider<HealthcareDeliveryContext> connectionProvider) 
            : base(connectionProvider)
        {
        }

        #endregion Constructors

        #region Methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.Connection);
        }

        protected override void ApplyConfigurations(ModelBuilder modelBuilder)
        {
            base.ApplyConfigurations(modelBuilder);
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.ApplyConfiguration(new SqlServerEventConfiguration());
            modelBuilder.ApplyConfiguration(new SqlServerPrescriptionStateConfiguration());
            modelBuilder.ApplyConfiguration(new SqlServerPrescribedMedicationStateConfiguration());
        }

        #endregion Methods

    }
}
