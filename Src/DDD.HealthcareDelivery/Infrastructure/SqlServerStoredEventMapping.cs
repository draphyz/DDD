namespace DDD.HealthcareDelivery.Infrastructure
{
    using Data = Core.Infrastructure.Data;

    public class SqlServerStoredEventMapping : Data.SqlServerStoredEventMapping
    {
        #region Constructors

        public SqlServerStoredEventMapping() : base(false)
        {
        }

        #endregion Constructors
    }
}
