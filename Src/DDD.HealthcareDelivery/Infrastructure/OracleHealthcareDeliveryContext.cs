using Microsoft.EntityFrameworkCore;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Prescriptions;
    using Core.Infrastructure.Data;

    public class OracleHealthcareDeliveryContext : DbHealthcareDeliveryContext
    {

        #region Constructors

        public OracleHealthcareDeliveryContext(DbContextOptions<DbHealthcareDeliveryContext> options) 
            : base(options)
        {
        }

        #endregion Constructors

        #region Methods

        protected override void ApplyConfigurations(ModelBuilder modelBuilder)
        {
            base.ApplyConfigurations(modelBuilder);
            modelBuilder.ApplyConfiguration(new OracleEventConfiguration());
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
