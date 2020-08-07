using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain.Prescriptions;

    internal class SqlServerPrescribedMedicationStateConfiguration : PrescribedMedicationStateConfiguration
    {

        #region Methods

        public override void Configure(EntityTypeBuilder<PrescribedMedicationState> builder)
        {
            base.Configure(builder);
            // Fields
            builder.Property(m => m.Identifier)
                   .HasDefaultValueSql("NEXT VALUE FOR PrescMedicationId");
        }

        #endregion Methods

    }
}