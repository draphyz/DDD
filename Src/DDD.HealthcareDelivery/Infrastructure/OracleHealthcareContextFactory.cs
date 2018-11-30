using Conditions;
using System.Threading.Tasks;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Threading;
    using Core.Infrastructure.Data;

    public class OracleHealthcareContextFactory : IAsyncDbContextFactory<HealthcareContext>
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

        public async Task<HealthcareContext> CreateContextAsync()
        {
            await new SynchronizationContextRemover();
            return new OracleHealthcareContext(await this.connectionFactory.CreateOpenConnectionAsync(), true);
        }

        #endregion Methods

    }
}
