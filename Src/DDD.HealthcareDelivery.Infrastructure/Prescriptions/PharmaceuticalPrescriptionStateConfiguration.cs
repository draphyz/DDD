using System.Data.Entity.ModelConfiguration;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain.Prescriptions;

    internal class PharmaceuticalPrescriptionStateConfiguration : EntityTypeConfiguration<PharmaceuticalPrescriptionState>
    {

        #region Fields

        private readonly bool useUpperCase;

        #endregion Fields

        #region Constructors

        public PharmaceuticalPrescriptionStateConfiguration(bool useUpperCase)
        {
            this.useUpperCase = useUpperCase;
            // Table
            this.Map(p => { p.Requires(ToCasingConvention(PrescriptionStateConfiguration.Discriminator)).HasValue("PHARM"); });
            // Relationships
            this.HasMany(p => p.PrescribedMedications).WithRequired().HasForeignKey(m => m.PrescriptionIdentifier);
        }

        #endregion Constructors

        #region Methods

        protected string ToCasingConvention(string name) => this.useUpperCase ? name.ToUpperInvariant() : name;

        #endregion Methods

    }
}
