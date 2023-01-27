using Microsoft.EntityFrameworkCore;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Infrastructure.Data;
    using Domain.Prescriptions;
    using Prescriptions;

    public abstract class DbHealthcareDeliveryContext : DbBoundedContext
    {

        #region Constructors

        protected DbHealthcareDeliveryContext(DbContextOptions options) 
            : base(options)
        {
        }

        #endregion Constructors

        #region Properties

        public virtual DbSet<PharmaceuticalPrescriptionState> PharmaceuticalPrescriptions { get; set; }

        #endregion Properties

        #region Methods

        protected override void ApplyConfigurations(ModelBuilder modelBuilder)
        {
            base.ApplyConfigurations(modelBuilder);
            modelBuilder.HasSequence<int>("PrescMedicationId");
            modelBuilder.ApplyConfiguration(new PharmaceuticalPrescriptionStateConfiguration());
        }

        protected override void ApplyConventions(ModelBuilder modelBuilder)
        {
            base.ApplyConventions(modelBuilder);
            modelBuilder.ApplyNonUnicodeStringsConvention();
        }

        #endregion Methods

    }
}
