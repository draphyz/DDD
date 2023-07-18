using System;

namespace DDD.Core.Application
{
    using Domain;
    using Serialization;

    public class RecurringCommandManagerSettings<TContext>
        where TContext : BoundedContext
    {

        #region Constructors

        public RecurringCommandManagerSettings(SerializationFormat currentSerializationFormat, 
                                               RecurringExpressionFormat currentExpressionFormat)
        {
            this.ContextType= typeof(TContext);
            this.CurrentSerializationFormat = currentSerializationFormat;
            this.CurrentExpressionFormat = currentExpressionFormat;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the type of the associated context.
        /// </summary>
        public Type ContextType { get; }

        /// <summary>
        /// Gets the current serialization format of the recurring commands.
        /// </summary>
        public SerializationFormat CurrentSerializationFormat { get; }

        /// <summary>
        /// Gets the current recurring expression format.
        /// </summary>
        public RecurringExpressionFormat CurrentExpressionFormat { get; }

        #endregion Properties

        #region Methods

        public override string ToString()
            => $"{this.GetType().Name} [{nameof(ContextType)}={ContextType}, {nameof(CurrentSerializationFormat)}={CurrentSerializationFormat}, {nameof(CurrentExpressionFormat)}={CurrentExpressionFormat}]";

        #endregion Methods
    }
}