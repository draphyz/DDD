using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.Core.Infrastructure.Data
{
    using Domain;

    public class SqlServerEventStateConfiguration : EventStateConfiguration
    {

        #region Methods

        public override void Configure(EntityTypeBuilder<EventState> builder)
        {
            base.Configure(builder);
            // Fields
            builder.Property(e => e.Id)
                   .HasDefaultValueSql("NEXT VALUE FOR EventId");
            builder.Property(e => e.OccurredOn)
                   .HasColumnType("datetime2(2)");
            builder.Property(e => e.Body)
                   .HasColumnType("xml");
        }

        #endregion Methods

    }
}