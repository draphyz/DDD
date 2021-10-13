using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.Core.Infrastructure.Data
{
    public class OracleStoredEventConfiguration : StoredEventConfiguration
    {

        #region Methods

        public override void Configure(EntityTypeBuilder<StoredEvent> builder)
        {
            base.Configure(builder);
            // Fields
            // Not supported by the 11.2 Oracle databases
            //builder.Property(e => e.Id)
            //       .HasDefaultValueSql("EventId.NEXTVAL");
            builder.Property(e => e.OccurredOn)
                   .HasColumnType("timestamp(3)"); // in milliseconds
        }

        #endregion Methods

    }
}