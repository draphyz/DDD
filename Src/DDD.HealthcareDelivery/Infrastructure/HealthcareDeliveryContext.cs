using Microsoft.EntityFrameworkCore;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Infrastructure.Data;
    using Core.Domain;
    using Domain.Prescriptions;
    using Prescriptions;

    public abstract class HealthcareDeliveryContext : StateEntitiesContext
    {

        #region Constructors

        protected HealthcareDeliveryContext(string connectionString) : base(connectionString)
        {
        }

        #endregion Constructors

        #region Properties

        public virtual DbSet<EventState> Events { get; set; }

        public virtual DbSet<PharmaceuticalPrescriptionState> PharmaceuticalPrescriptions { get; set; }

        #endregion Properties

        #region Methods

        protected override void ApplyConfigurations(ModelBuilder modelBuilder)
        {
            base.ApplyConfigurations(modelBuilder);
            modelBuilder.HasSequence<long>("EventId");
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
