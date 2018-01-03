namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    internal class SqlServerStoredEventConfiguration : StoredEventConfiguration
    {

        #region Constructors

        public SqlServerStoredEventConfiguration(bool useUpperCase) : base(useUpperCase)
        {
            // Fields
            this.Property(e => e.OccurredOn)
                .HasColumnType("datetime2");
            this.Property(e => e.Body)
                .HasColumnType("xml");
        }

        #endregion Constructors

    }
}