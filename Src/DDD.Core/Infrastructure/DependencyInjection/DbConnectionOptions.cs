using EnsureThat;
using System.Runtime.Serialization;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Domain;

    [DataContract]
    public class DbConnectionOptions
    {

        #region Constructors

        private DbConnectionOptions(string contextType)
        {
            this.ContextType = contextType;
        }

        /// <remarks>
        /// For serialization
        /// </remarks>
        private DbConnectionOptions() { }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the type name of the associated context.
        /// </summary>
        [DataMember(Order = 1)]
        public string ContextType { get; private set; }

        /// <summary>
        /// Gets the provider name.
        /// </summary>
        [DataMember(Order = 2)]
        public string ProviderName { get; private set; }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        [DataMember(Order = 3)]
        public string ConnectionString { get; private set; }

        #endregion Properties

        #region Classes

        public class Builder<TContext> : IObjectBuilder<DbConnectionOptions>
            where TContext : BoundedContext
        {

            #region Fields

            private readonly DbConnectionOptions options;

            #endregion Fields

            #region Constructors

            public Builder()
            {
                this.options = new DbConnectionOptions(typeof(TContext).ShortAssemblyQualifiedName());
            }

            #endregion Constructors

            #region Methods

            public Builder<TContext> SetProviderName(string providerName)
            {
                Ensure.That(providerName, nameof(providerName)).IsNotNullOrWhiteSpace();
                options.ProviderName = providerName;
                return this;
            }

            public Builder<TContext> SetConnectionString(string connectionString)
            {
                Ensure.That(connectionString, nameof(connectionString)).IsNotNullOrWhiteSpace();
                options.ConnectionString = connectionString;
                return this;
            }

            DbConnectionOptions IObjectBuilder<DbConnectionOptions>.Build() => this.options;

            #endregion Methods

        }

        #endregion Classes

    }
}
