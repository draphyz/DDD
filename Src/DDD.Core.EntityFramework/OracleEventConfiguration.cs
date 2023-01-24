using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.Core.Infrastructure.Data
{
    using Application;

    public class OracleEventConfiguration : EventConfiguration
    {

        #region Methods

        public override void Configure(EntityTypeBuilder<Event> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.OccurredOn)
                   .HasColumnType("timestamp(3)"); // in milliseconds
        }

        #endregion Methods

    }
}