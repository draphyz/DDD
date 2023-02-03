using EnsureThat;
using System;
using System.Resources;
using System.Linq;
using System.Diagnostics;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace DDD.Core.Infrastructure.Testing
{
    using Data;
    using Domain;

    public abstract class DbFixture<TContext> : IDbFixture<TContext>
        where TContext : BoundedContext, new()
    {

        #region Fields

        private readonly ResourceManager resourceManager;
        private readonly ConnectionStringSettings connectionSettings;

        #endregion Fields

        #region Constructors

        protected DbFixture(string resourceFile, ConnectionStringSettings connectionSettings)
        {
            Ensure.That(resourceFile, nameof(resourceFile)).IsNotNullOrWhiteSpace();
            Ensure.That(connectionSettings, nameof(connectionSettings)).IsNotNull();
            this.LoadConfiguration();
            var resourceType = GetResourceType(resourceFile);
            this.resourceManager = new ResourceManager(resourceType);
            this.connectionSettings = connectionSettings;
            this.CreateDatabase();
        }

        #endregion Constructors

        #region Methods

        public int[] ExecuteScript(string script)
        {
            using (var connection = this.CreateConnection())
            {
                connection.Open();
                return this.ExecuteScript(script, connection);
            }
        }

        public int[] ExecuteScriptFromResources(string scriptName)
        {
            Ensure.That(scriptName, nameof(scriptName)).IsNotNullOrWhiteSpace();
            var script = this.resourceManager.GetString(scriptName);
            return this.ExecuteScript(script);
        }

        public IDbConnectionProvider<TContext> CreateConnectionProvider(bool pooling = true)
        {
            var connectionString = this.SetPooling(connectionSettings.ConnectionString, pooling);
            return new LazyDbConnectionProvider<TContext>(connectionSettings.ProviderName, connectionString);
        }

        public DbConnection CreateConnection(bool pooling = true)
        {
            var connectionString = this.SetPooling(connectionSettings.ConnectionString, pooling);
            var providerFactory = DbProviderFactories.GetFactory(connectionSettings.ProviderName);
            var connection = providerFactory.CreateConnection();
            connection.ConnectionString = connectionString;
            return connection;
        }

        protected abstract string SetPooling(string connectionString, bool pooling);

        protected abstract int[] ExecuteScript(string script, IDbConnection connection);

        protected abstract void CreateDatabase();

        protected virtual void LoadConfiguration()
        { 
        }

        private static Type GetResourceType(string resourceFile)
        {
            var callingAssemblies = new StackTrace().GetFrames()
                                                    .Select(x => x.GetMethod().ReflectedType.Assembly)
                                                    .Distinct();
            foreach (var assembly in callingAssemblies)
            {
                var resourceType = assembly.GetTypes().FirstOrDefault(t => t.Name == resourceFile);
                if (resourceType != null)
                    return resourceType;
            }
            throw new ArgumentException($"Cannot find the resource file '{resourceFile}'", nameof(resourceFile));
        }

        #endregion Methods

    }
}