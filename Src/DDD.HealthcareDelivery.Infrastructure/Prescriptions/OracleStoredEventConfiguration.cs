namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    internal class OracleStoredEventConfiguration : StoredEventConfiguration
    {

        #region Constructors

        public OracleStoredEventConfiguration(bool useUpperCase) : base(useUpperCase)
        {
            // Fields
            this.Property(e => e.OccurredOn)
                .HasColumnType("timestamp");
            this.Property(e => e.Body)
                .HasColumnType("xmltype");
        }

        #endregion Constructors

    }
}