using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.Core.Infrastructure.Data
{
    using Domain;

    public abstract class EventStateConfiguration : IEntityTypeConfiguration<EventState>
    {

        #region Methods

        public virtual void Configure(EntityTypeBuilder<EventState> builder)
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
                   .HasMaxLength(50)
                   .IsRequired();
            builder.Property(e => e.StreamId)
                   .IsUnicode(false)
                   .HasMaxLength(50)
                   .IsRequired();
            builder.Property(e => e.CommitId);
            builder.Property(e => e.Subject)
                   .IsUnicode(false)
                   .HasMaxLength(100);
            builder.Property(e => e.Body)
                   .IsRequired();
            builder.Property(e => e.Dispatched);
        }

        #endregion Methods

    }
}