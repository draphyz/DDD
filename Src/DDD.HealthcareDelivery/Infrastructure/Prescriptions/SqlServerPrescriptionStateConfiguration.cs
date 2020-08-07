using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain.Prescriptions;

    internal class SqlServerPrescriptionStateConfiguration : PrescriptionStateConfiguration
    {

        #region Constructors

        public override void Configure(EntityTypeBuilder<PrescriptionState> builder)
        {
            base.Configure(builder);
            // Fields
            builder.Property(p => p.CreatedOn)
                   .HasColumnType("smalldatetime");
        }

        #endregion Constructors

    }
}
