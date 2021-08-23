using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain.Prescriptions;

    internal abstract class PrescribedMedicationStateConfiguration : IEntityTypeConfiguration<PrescribedMedicationState> 
    {

        #region Constructors

        public virtual void Configure(EntityTypeBuilder<PrescribedMedicationState> builder)
        {
            // Table
            builder.ToTable("PrescMedication");
            // Keys
            builder.HasKey(m => m.Identifier);
            //// Indexes
            builder.HasIndex(m => m.PrescriptionIdentifier)
                   .HasName("IX_PRESCMED_PRESCID"); // Identifiers limited to 30 characters in Oracle 11g
            // Fields
            builder.Property(m => m.Identifier)
                   .HasColumnName("PrescMedicationId");
            builder.Property(m => m.PrescriptionIdentifier)
                   .HasColumnName("PrescriptionId");
            builder.Property(m => m.MedicationType)
                   .HasMaxLength(20)
                   .IsRequired();
            builder.Property(m => m.NameOrDescription)
                   .HasColumnName("NameOrDesc")
                   .HasMaxLength(1024)
                   .IsRequired();
            builder.Property(m => m.Posology)
                   .HasMaxLength(1024);
            builder.Property(m => m.Code)
                   .HasMaxLength(20);
        }

        #endregion Constructors

    }
}
