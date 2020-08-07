using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain.Prescriptions;

    internal class PharmaceuticalPrescriptionStateConfiguration : IEntityTypeConfiguration<PharmaceuticalPrescriptionState>
    {

        #region Methods

        public void Configure(EntityTypeBuilder<PharmaceuticalPrescriptionState> builder)
        {
            // Relationships
            builder.HasMany(p => p.PrescribedMedications).WithOne().IsRequired()
                                                                   .HasForeignKey(m => m.PrescriptionIdentifier)
                                                                   .HasConstraintName("FK_PRESCMED_PRESC_PRESCID"); // Identifiers limited to 30 characters in Oracle 11g
        }

        #endregion Methods

    }
}
 