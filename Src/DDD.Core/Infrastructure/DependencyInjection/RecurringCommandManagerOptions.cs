using System.Runtime.Serialization;
using DDD.Serialization;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Domain;
    using Application;

    [DataContract]
    public class RecurringCommandManagerOptions
    {

        #region Constructors

        private RecurringCommandManagerOptions(string contextType) 
        {
            this.ContextType = contextType;
        }

        /// <remarks>
        /// For serialization
        /// </remarks>
        private RecurringCommandManagerOptions() { }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the type name of the associated context.
        /// </summary>
        [DataMember(Order = 1)]
        public string ContextType { get; private set; }

        /// <summary>
        /// Gets the current serialization format of recurring commands.
        /// </summary>
        [DataMember(Order = 2)]
        public SerializationFormat CurrentSerializationFormat { get; private set; }

        /// <summary>
        /// Gets the current recurring expression format.
        /// </summary>
        [DataMember(Order = 3)]
        public RecurringExpressionFormat CurrentExpressionFormat { get; private set; }

        #endregion Properties

        #region Classes

        public class Builder<TContext> : IObjectBuilder<RecurringCommandManagerOptions>
            where TContext : BoundedContext
        {

            #region Fields

            private readonly RecurringCommandManagerOptions options;

            #endregion Fields

            public Builder() 
            {
                this.options = new RecurringCommandManagerOptions(typeof(TContext).ShortAssemblyQualifiedName());
            }

            #region Methods

            public Builder<TContext> SetCurrentSerializationFormat(SerializationFormat format)
            {
                this.options.CurrentSerializationFormat = format;
                return this;
            }

            public Builder<TContext> SetCurrentExpressionFormat(RecurringExpressionFormat format)
            {
                this.options.CurrentExpressionFormat = format;
                return this;
            }

            RecurringCommandManagerOptions IObjectBuilder<RecurringCommandManagerOptions>.Build() => this.options;

            #endregion Methods

        }

        #endregion Classes

    }
}
