using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.Core.Infrastructure.Data
{
    public abstract class StoredEventConfiguration : IEntityTypeConfiguration<StoredEvent>
    {

        #region Methods

        public virtual void Configure(EntityTypeBuilder<StoredEvent> builder)
        {
            // Table
            builder.ToTable("Event");
            // Keys
            builder.HasKey(e => e.Id);
            // Fields
            builder.Property(e => e.Id)
                   .HasColumnName("EventId");
            builder.Property(e => e.EventType)
                   .IsUnicode(false)
                   .HasMaxLength(250)
                   .IsRequired();
            builder.Property(e => e.Version);
            builder.Property(e => e.Body)
                   .IsUnicode(false)
                   .IsRequired();
            builder.Property(e => e.StreamId)
                   .IsUnicode(false)
                   .HasMaxLength(50)
                   .IsRequired();
            builder.Property(e => e.StreamType)
                   .IsUnicode(false)
                   .HasMaxLength(50)
                   .IsRequired();
            builder.Property(e => e.IssuedBy)
                   .IsUnicode(false)
                   .HasMaxLength(100);
        }

        #endregion Methods

    }
}