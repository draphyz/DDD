using Conditions;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Infrastructure.Data;

    public class SqlServerHealthcareContextFactory : IDbContextFactory<HealthcareContext>
    {

        #region Fields

        private readonly IHealthcareConnectionFactory connectionFactory;

        #endregion Fields

        #region Constructors

        public SqlServerHealthcareContextFactory(IHealthcareConnectionFactory connectionFactory)
        {
            Condition.Requires(connectionFactory, nameof(connectionFactory)).IsNotNull();
            this.connectionFactory = connectionFactory;
        }

        #endregion Constructors

        #region Methods

        public HealthcareContext CreateContext()
        {
            return new SqlServerHealthcareContext(this.connectionFactory.CreateOpenConnection(), true);
        }

        #endregion Methods

    }
}
