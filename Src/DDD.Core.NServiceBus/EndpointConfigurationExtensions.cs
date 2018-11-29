using Conditions;
using NServiceBus;

namespace DDD.Core.Infrastructure.Messaging
{
    using Application;
    using Domain;

    public static class EndpointConfigurationExtensions
    {

        #region Methods

        /// <summary>
        /// Uses custom marker interfaces to detect messages, commands and events.
        /// </summary>
        public static void UseCustomMarkerInterfaces(this EndpointConfiguration endpointConfiguration)
        {
            Condition.Requires(endpointConfiguration, nameof(endpointConfiguration)).IsNotNull();
            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningMessagesAs(
            type =>
            {
                return typeof(IMessage).IsAssignableFrom(type) &&
                       typeof(IMessage) != type &&
                       typeof(IEvent) != type &&
                       typeof(ICommand) != type;
            });
            conventions.DefiningCommandsAs(
                type =>
                {
                    return typeof(ICommand).IsAssignableFrom(type) &&
                           typeof(ICommand) != type;
                });
            conventions.DefiningEventsAs(
                type =>
                {
                    return typeof(IEvent).IsAssignableFrom(type) &&
                           typeof(IEvent) != type;
                });
        }

        #endregion Methods

    }
}
