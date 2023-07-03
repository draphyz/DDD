using EnsureThat;

namespace DDD.Core.Application
{
    using Domain;
    using Serialization;

    /// <remarks>
    /// Used for dependency injection
    /// </remarks>
    public class RecurringCommandManagerSettings<TContext>
        where TContext : BoundedContext
    {

        #region Constructors

        public RecurringCommandManagerSettings(TContext context,
                                               SerializationFormat currentSerializationFormat)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            this.Context= context;
            this.CurrentSerializationFormat = currentSerializationFormat;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the associated context.
        /// </summary>
        public TContext Context { get; }

        /// <summary>
        /// Gets the current serialization format of the recurring commands.
        /// </summary>
        public SerializationFormat CurrentSerializationFormat { get; }

        #endregion Properties
    }
}