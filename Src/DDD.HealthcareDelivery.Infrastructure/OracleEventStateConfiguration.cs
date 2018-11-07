namespace DDD.HealthcareDelivery.Infrastructure
{
    internal class OracleEventStateConfiguration : EventStateConfiguration
    {

        #region Constructors

        public OracleEventStateConfiguration(bool useUpperCase) : base(useUpperCase)
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