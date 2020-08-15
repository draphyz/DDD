using Conditions;
using System.Data;
using System.Resources;
using System.Reflection;
using System.Linq;

namespace DDD.Core.Infrastructure.Testing
{
    using Data;

    public abstract class DbFixture<TConnectionFactory> : IDbFixture<TConnectionFactory>
        where TConnectionFactory : class, IDbConnectionFactory
    {

        #region Fields

        private readonly ResourceManager resourceManager;

        #endregion Fields

        #region Constructors

        protected DbFixture(TConnectionFactory connectionFactory, string resourceFile)
        {
            Condition.Requires(connectionFactory, nameof(connectionFactory)).IsNotNull();
            Condition.Requires(resourceFile, nameof(resourceFile)).IsNotNullOrWhiteSpace();
            this.ConnectionFactory = connectionFactory;
            var resourceAssembly = Assembly.GetCallingAssembly();
            var resourceType = resourceAssembly.GetTypes().Single(t => t.Name == resourceFile);
            this.resourceManager = new ResourceManager(resourceType);
            this.RegisterDbProviderFactory();
            this.CreateDatabase();
        }

        #endregion Constructors

        #region Properties

        public TConnectionFactory ConnectionFactory { get; }

        #endregion Properties

        #region Methods

        public int[] ExecuteScript(string script)
        {
            Condition.Requires(script, nameof(script)).IsNotNullOrWhiteSpace();
            using (var connection = this.ConnectionFactory.CreateOpenConnection())
            {
                return this.ExecuteScript(script, connection);
            }
        }

        public int[] ExecuteScriptFromResources(string scriptName)
        {
            Condition.Requires(scriptName, nameof(scriptName)).IsNotNullOrWhiteSpace();
            var script = this.resourceManager.GetString(scriptName);
            return this.ExecuteScript(script);
        }

        protected abstract void CreateDatabase();

        protected abstract int[] ExecuteScript(string script, IDbConnection connection);

        protected virtual void RegisterDbProviderFactory()
        {
        }

        #endregion Methods
    }
}