using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using DDD.Serialization;

namespace DDD.Core.Infrastructure.Data
{
    using Application;

    public abstract class EventConfiguration : IEntityTypeConfiguration<Event>
    {

        #region Methods

        public virtual void Configure(EntityTypeBuilder<Event> builder)
        {
            // Table
            builder.ToTable("Event");
            // Keys
            builder.HasKey(e => e.EventId);
            // Fields
            builder.Property(e => e.EventId)
                   .ValueGeneratedNever();
            builder.Property(e => e.EventType)
                   .IsUnicode(false)
                   .HasMaxLength(250)
                   .IsRequired();
            builder.Property(e => e.Body)
                   .IsUnicode(false)
                   .IsRequired();
            builder.Property(e => e.BodyFormat)
                   .IsUnicode(false)
                   .HasMaxLength(20)
                   .IsRequired()
                   .HasConversion<string>();
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