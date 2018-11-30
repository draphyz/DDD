using Conditions;
using System.Threading.Tasks;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Threading;
    using Core.Infrastructure.Data;

    public class SqlServerHealthcareContextFactory : IAsyncDbContextFactory<HealthcareContext>
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

        public async Task<HealthcareContext> CreateContextAsync()
        {
            await new SynchronizationContextRemover();
            return new SqlServerHealthcareContext(await this.connectionFactory.CreateOpenConnectionAsync(), true);
        }

        #endregion Methods

    }
}
