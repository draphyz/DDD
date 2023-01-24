using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.Core.Infrastructure.Data
{
    using Application;

    public class SqlServerEventConfiguration : EventConfiguration
    {

        #region Methods

        public override void Configure(EntityTypeBuilder<Event> builder)
        {
            base.Configure(builder);
            // Fields
            builder.Property(e => e.OccurredOn)
                   .HasColumnType("datetime2(3)"); // in milliseconds
        }

        #endregion Methods

    }
}