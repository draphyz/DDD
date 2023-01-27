using Microsoft.EntityFrameworkCore;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Infrastructure.Data;
    using Prescriptions;

    public class SqlServerHealthcareDeliveryContext : DbHealthcareDeliveryContext
    {

        #region Constructors

        public SqlServerHealthcareDeliveryContext(DbContextOptions<DbHealthcareDeliveryContext> options) 
            : base(options)
        {
        }

        #endregion Constructors

        #region Methods

        protected override void ApplyConfigurations(ModelBuilder modelBuilder)
        {
            base.ApplyConfigurations(modelBuilder);
            modelBuilder.ApplyConfiguration(new SqlServerEventConfiguration());
            modelBuilder.ApplyConfiguration(new SqlServerPrescriptionStateConfiguration());
            modelBuilder.ApplyConfiguration(new SqlServerPrescribedMedicationStateConfiguration());
        }

        #endregion Methods

    }
}
