using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Core.Infrastructure.Serialization;

    internal abstract class StoredEventConfiguration : EntityTypeConfiguration<StoredEvent>
    {

        #region Fields

        private readonly bool useUpperCase;

        #endregion Fields

        #region Constructors

        protected StoredEventConfiguration(bool useUpperCase)
        {
            this.useUpperCase = useUpperCase;
            // Table
            this.ToTable(ToCasingConvention("Event"));
            // Keys
            this.HasKey(e => e.Id);
            // Fields
            this.Property(e => e.Id)
                .HasColumnName(ToCasingConvention("EventId"))
                .HasColumnOrder(1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(e => e.EventType)
                .HasColumnOrder(2)
                .IsUnicode(false)
                .HasMaxLength(50)
                .IsRequired();
            this.Property(e => e.StreamId)
                .HasColumnOrder(3)
                .IsUnicode(false)
                .HasMaxLength(50)
                .IsRequired();
            this.Property(e => e.CommitId)
                .HasColumnOrder(4);
            this.Property(e => e.OccurredOn)
                .HasColumnOrder(5)
                .HasPrecision(2);
            this.Property(e => e.Subject)
                .HasColumnOrder(6)
                .IsUnicode(false)
                .HasMaxLength(100);
            this.Property(e => e.Body)
                .HasColumnOrder(7)
                .IsRequired();
            this.Property(e => e.Dispatched)
                .HasColumnOrder(8);
        }

        #endregion Constructors

        #region Methods

        protected string ToCasingConvention(string name) => this.useUpperCase ? name.ToUpperInvariant() : name;

        #endregion Methods

    }
}