using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain.Prescriptions;

    internal abstract class PrescribedMedicationMapping : ClassMapping<PrescribedMedication>
    {

        #region Fields

        private readonly bool useUpperCase;

        #endregion Fields

        #region Constructors

        protected PrescribedMedicationMapping(bool useUpperCase)
        {
            this.useUpperCase = useUpperCase;
            this.Lazy(false);
            // Table
            this.Table(ToCasingConvention("PrescMedication"));
            // Keys
            this.Id("identifier", m => m.Column(ToCasingConvention("PrescMedicationId")));
            // Fields

        }

            //this.Property(m => m.PrescriptionIdentifier)
            //    .HasColumnName(ToCasingConvention("PrescriptionId"))
            //    .HasColumnOrder(2);
            //this.Property(m => m.MedicationType)
            //    .HasColumnOrder(3)
            //    .IsUnicode(false)
            //    .HasMaxLength(20)
            //    .IsRequired();
            //this.Property(m => m.NameOrDescription)
            //    .HasColumnName(ToCasingConvention("NameOrDesc"))
            //    .HasColumnOrder(4)
            //    .IsUnicode(false)
            //    .HasMaxLength(1024)
            //    .IsRequired();
            //this.Property(m => m.Posology)
            //    .HasColumnOrder(5)
            //    .IsUnicode(false)
            //    .HasMaxLength(1024);
            //this.Property(m => m.Quantity)
            //    .HasColumnOrder(6)
            //    .IsUnicode(false)
            //    .HasMaxLength(100); ;
            //this.Property(m => m.Duration)
            //    .HasColumnOrder(7)
            //    .IsUnicode(false)
            //    .HasMaxLength(100);
            //this.Property(m => m.Code)
            //    .HasColumnOrder(8)
            //    .IsUnicode(false)
            //    .HasMaxLength(20);

        #endregion Constructors

        #region Methods

        protected string ToCasingConvention(string name) => this.useUpperCase ? name.ToUpperInvariant() : name;

        #endregion Methods

    }
}
