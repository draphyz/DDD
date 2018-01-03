using Conditions;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Infrastructure.Data;

    public class OracleHealthcareContextFactory : IDbContextFactory<HealthcareContext>
    {

        #region Fields

        private readonly IHealthcareConnectionFactory connectionFactory;

        #endregion Fields

        #region Constructors

        public OracleHealthcareContextFactory(IHealthcareConnectionFactory connectionFactory)
        {
            Condition.Requires(connectionFactory, nameof(connectionFactory)).IsNotNull();
            this.connectionFactory = connectionFactory;
        }

        #endregion Constructors

        #region Methods

        public HealthcareContext CreateContext()
        {
            return new OracleHealthcareContext(this.connectionFactory.CreateOpenConnection(), true);
        }

        #endregion Methods

    }
}
