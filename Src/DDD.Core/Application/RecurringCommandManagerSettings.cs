﻿using System.Runtime.Serialization;
using EnsureThat;

namespace DDD.Core.Application
{
    using Serialization;

    /// <remarks>
    /// Used for serialization
    /// </remarks>
    [DataContract]
    public class RecurringCommandManagerSettings
    {

        #region Constructors

        public RecurringCommandManagerSettings(string context,
                                               SerializationFormat currentSerializationFormat)
        {
            Ensure.That(context, nameof(context)).IsNotNullOrWhiteSpace();
            this.Context= context;
            this.CurrentSerializationFormat = currentSerializationFormat;
        }

        /// <remarks>
        /// For serialization
        /// </remarks>
        private RecurringCommandManagerSettings() { }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the associated context.
        /// </summary>
        [DataMember(Order = 1)]
        public string Context { get; private set; }

        /// <summary>
        /// Gets the current serialization format of the recurring commands.
        /// </summary>
        [DataMember(Order = 2)]
        public SerializationFormat CurrentSerializationFormat { get; private set; }

        #endregion Properties
    }
}