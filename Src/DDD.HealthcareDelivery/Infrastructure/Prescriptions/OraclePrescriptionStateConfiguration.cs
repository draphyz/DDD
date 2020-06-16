namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    internal class OraclePrescriptionStateConfiguration : PrescriptionStateConfiguration
    {

        #region Constructors

        public OraclePrescriptionStateConfiguration(bool useUpperCase) : base(useUpperCase)
        {
            // Table
            this.Map(p =>
            {
                p.ToTable(ToCasingConvention(TableName));
                p.Requires(ToCasingConvention(Discriminator))
                 .HasValue(string.Empty)
                 .HasColumnOrder(2)
                 .HasColumnType("varchar2")
                 .HasMaxLength(5);
            });
            // Fields
            this.Property(p => p.CreatedOn)
                .HasColumnType("date");
            this.Property(p => p.DeliverableAt)
                .HasColumnType("date");
            this.Property(p => p.Patient.Birthdate)
                .HasColumnType("date");
        }

        #endregion Constructors

    }
}
