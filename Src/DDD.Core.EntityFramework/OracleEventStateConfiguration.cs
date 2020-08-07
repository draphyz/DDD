using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.Core.Infrastructure.Data
{
    using Domain;

    public class OracleEventStateConfiguration : EventStateConfiguration
    {

        #region Methods

        public override void Configure(EntityTypeBuilder<EventState> builder)
        {
            base.Configure(builder);
            // Fields
            // Not supported by the 11.2 Oracle databases
            //builder.Property(e => e.Id)
            //       .HasDefaultValueSql("EventId.NEXTVAL");
            builder.Property(e => e.OccurredOn)
                   .HasColumnType("timestamp(6)");
            builder.Property(e => e.Body)
                   .HasColumnType("xmltype");
        }

        #endregion Methods

    }
}