using EnsureThat;
using System;

namespace DDD.Mapping
{
    using Collections;

    public static class IMappingContextExtensions
    {

        #region Methods

        public static Type DestinationType(this IMappingContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            context.TryGetValue(MappingContextInfo.DestinationType, out Type destinationType);
            return destinationType;
        }

        public static void AddDestinationType(this IMappingContext context, Type destinationType)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            Ensure.That(destinationType, nameof(destinationType)).IsNotNull();
            context.Add(MappingContextInfo.DestinationType, destinationType);
        }

        #endregion Methods

    }
}