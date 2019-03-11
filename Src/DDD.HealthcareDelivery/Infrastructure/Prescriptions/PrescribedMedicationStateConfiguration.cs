using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain.Prescriptions;

    internal class PrescribedMedicationStateConfiguration : EntityTypeConfiguration<PrescribedMedicationState> 
    {

        #region Fields

        private readonly bool useUpperCase;

        #endregion Fields

        #region Constructors

        public PrescribedMedicationStateConfiguration(bool useUpperCase)
        {
            this.useUpperCase = useUpperCase;
            // Table
            this.ToTable(ToCasingConvention("PrescMedication"));
            // Keys
            this.HasKey(m => m.Identifier);
            // Fields
            this.Property(m => m.Identifier)
                .HasColumnName(ToCasingConvention("PrescMedicationId"))
                .HasColumnOrder(1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(m => m.PrescriptionIdentifier)
                .HasColumnName(ToCasingConvention("PrescriptionId"))
                .HasColumnOrder(2);
            this.Property(m => m.MedicationType)
                .HasColumnOrder(3)
                .IsUnicode(false)
                .HasMaxLength(20)
                .IsRequired();
            this.Property(m => m.NameOrDescription)
                .HasColumnName(ToCasingConvention("NameOrDesc"))
                .HasColumnOrder(4)
                .IsUnicode(false)
                .HasMaxLength(1024)
                .IsRequired();
            this.Property(m => m.Posology)
                .HasColumnOrder(5)
                .IsUnicode(false)
                .HasMaxLength(1024);
            this.Property(m => m.Quantity)
                .HasColumnOrder(6)
                .IsUnicode(false)
                .HasMaxLength(100); ;
            this.Property(m => m.Duration)
                .HasColumnOrder(7)
                .IsUnicode(false)
                .HasMaxLength(100);
            this.Property(m => m.Code)
                .HasColumnOrder(8)
                .IsUnicode(false)
                .HasMaxLength(20);
        }

        #endregion Constructors

        #region Methods

        protected string ToCasingConvention(string name) => this.useUpperCase ? name.ToUpperInvariant() : name;

        #endregion Methods

    }
}
