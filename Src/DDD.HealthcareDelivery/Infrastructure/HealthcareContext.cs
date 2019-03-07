using System.Data.Entity;
using System.Data.Common;
using System.Linq;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Infrastructure.Data;
    using Core.Domain;
    using Domain.Prescriptions;
    using Domain.Patients;
    using Domain.Practitioners;
    using Prescriptions;
    using Common.Domain;

    public abstract class HealthcareContext : StateEntitiesContext
    {

        #region Constructors

        protected HealthcareContext(DbConnection connection, bool contextOwnsConnection) 
            : base(connection, contextOwnsConnection)
        {
        }

        #endregion Constructors

        #region Properties

        public virtual DbSet<EventState> Events { get; set; }

        public virtual DbSet<PharmaceuticalPrescriptionState> PharmaceuticalPrescriptions { get; set; }

        protected bool UseUpperCase { get; set; } = false;

        #endregion Properties

        #region Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            this.AddConfigurations(modelBuilder);
        }

        protected override void SetGeneratedValues()
        {
            this.SetIdsForEvents();
            this.SetIdsForPrescribedMedications();
        }

        private void AddConfigurations(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PharmaceuticalPrescriptionStateConfiguration(this.UseUpperCase));
            modelBuilder.Configurations.Add(new PrescribedMedicationStateConfiguration(this.UseUpperCase));
            modelBuilder.ComplexType<PatientState>().Ignore(p => p.ContactInformation);
            modelBuilder.ComplexType<ContactInformationState>().Ignore(c => c.FaxNumber);
            modelBuilder.ComplexType<HealthcarePractitionerState>();
        }
        private void SetIdsForEvents()
        {
            var events = this.ChangeTracker.Entries<EventState>()
                        .Select(m => m.Entity);
            foreach (var evt in events)
                evt.Id = this.Database.Connection.NextValue<int>("EventId");
        }

        private void SetIdsForPrescribedMedications()
        {
            var medications = this.ChangeTracker.Entries<PrescribedMedicationState>()
                                    .Where(m => m.Entity.EntityState == Core.Domain.EntityState.Added)
                                    .Select(m => m.Entity);
            foreach (var medication in medications)
                medication.Identifier = this.Database.Connection.NextValue<int>("PrescMedicationId");
        }

        #endregion Methods

    }
}
