using System.Runtime.Serialization;
using EnsureThat;

namespace DDD.Core.Application
{
    using Domain;
    using Serialization;

    [DataContract()]
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
        [DataMember(Order = 1)]
        public SerializationFormat CurrentSerializationFormat { get; }

        #endregion Properties
    }
}