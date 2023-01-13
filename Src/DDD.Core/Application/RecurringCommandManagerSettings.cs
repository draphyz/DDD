using System.Runtime.Serialization;

namespace DDD.Core.Application
{
    using Serialization;

    [DataContract()]
    public class RecurringCommandManagerSettings
    {

        #region Constructors

        public RecurringCommandManagerSettings(SerializationFormat currentSerializationFormat)
        {
            this.CurrentSerializationFormat = currentSerializationFormat;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the current serialization format of the recurring commands.
        /// </summary>
        [DataMember(Order = 1)]
        public SerializationFormat CurrentSerializationFormat { get; }

        #endregion Properties

    }
}