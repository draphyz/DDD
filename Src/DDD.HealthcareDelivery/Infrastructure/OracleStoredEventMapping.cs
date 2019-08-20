namespace DDD.HealthcareDelivery.Infrastructure
{
    using Data = Core.Infrastructure.Data;

    public class OracleStoredEventMapping : Data.OracleStoredEventMapping
    {
        #region Constructors

        public OracleStoredEventMapping() : base(true)
        {
        }

        #endregion Constructors
    }
}
