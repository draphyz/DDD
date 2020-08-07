using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain.Prescriptions;

    internal class OraclePrescribedMedicationStateConfiguration : PrescribedMedicationStateConfiguration
    {

        #region Methods

        public override void Configure(EntityTypeBuilder<PrescribedMedicationState> builder)
        {
            base.Configure(builder);
            // Fields
            // Not supported by the 11.2 Oracle databases
            //builder.Property(m => m.Identifier)
            //       .HasDefaultValueSql("PrescMedicationId.NEXTVAL"); 
        }

        #endregion Methods

    }
}