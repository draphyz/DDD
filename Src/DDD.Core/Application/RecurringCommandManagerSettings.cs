using System.Runtime.Serialization;

namespace DDD.Core.Application
{
    using Domain;
    using Serialization;

    [DataContract()]
    public class RecurringCommandManagerSettings<TContext>
        where TContext : BoundedContext, new()
    {
        #region Fields

        private readonly TContext context;

        #endregion Fields

        #region Constructors

        public RecurringCommandManagerSettings(SerializationFormat currentSerializationFormat)
        {
            this.CurrentSerializationFormat = currentSerializationFormat;
            this.context = new TContext();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the associated context.
        /// </summary>
        public TContext Context => this.context;

        /// <summary>
        /// Gets the current serialization format of the recurring commands.
        /// </summary>
        [DataMember(Order = 1)]
        public SerializationFormat CurrentSerializationFormat { get; }

        #endregion Properties
    }
}