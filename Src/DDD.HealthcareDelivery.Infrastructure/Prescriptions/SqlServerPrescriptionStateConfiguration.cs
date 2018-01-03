namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    internal class SqlServerPrescriptionStateConfiguration : PrescriptionStateConfiguration
    {

        #region Constructors

        public SqlServerPrescriptionStateConfiguration(bool useUpperCase) : base(useUpperCase)
        {
            // Table
            this.Map(p =>
            {
                p.ToTable(ToCasingConvention(TableName));
                p.Requires(ToCasingConvention(Discriminator))
                 .HasValue(string.Empty)
                 .HasColumnOrder(2)
                 .HasColumnType("varchar")
                 .HasMaxLength(5);
            });
            // Fields
            this.Property(p => p.CreatedOn)
                .HasColumnType("smalldatetime");
            this.Property(p => p.DelivrableAt)
                .HasColumnType("date");
            this.Property(p => p.Patient.Birthdate)
                .HasColumnType("date");
        }

        #endregion Constructors

    }
}
