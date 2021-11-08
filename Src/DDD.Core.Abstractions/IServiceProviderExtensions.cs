using Conditions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DDD
{
    /// <summary>
    /// Adds extension methods to the <see cref="IServiceProvider" /> interface.
    /// </summary>
    public static class IServiceProviderExtensions
    {

        #region Methods

        /// <summary>
        /// Get service of type <typeparamref name="T"/> from the <see cref="IServiceProvider"/>.
        /// </summary>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <param name="provider">The <see cref="IServiceProvider"/> to retrieve the service object from.</param>
        /// <returns>A service object of type <typeparamref name="T"/> or null if there is no such service.</returns>
        public static T GetService<T>(this IServiceProvider provider)
        {
            Condition.Requires(provider, nameof(provider)).IsNotNull();
            return (T)provider.GetService(typeof(T));
        }

        /// <summary>
        /// Get an enumeration of services of type <typeparamref name="T"/> from the <see cref="IServiceProvider"/>.
        /// </summary>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <param name="provider">The <see cref="IServiceProvider"/> to retrieve the services from.</param>
        /// <returns>An enumeration of services of type <typeparamref name="T"/>.</returns>
        public static IEnumerable<T> GetServices<T>(this IServiceProvider provider)
        {
            Condition.Requires(provider, nameof(provider)).IsNotNull();
            return provider.GetService<IEnumerable<T>>() ?? Enumerable.Empty<T>();
        }

        /// <summary>
        /// Get an enumeration of services of type <paramref name="serviceType"/> from the <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="provider">The <see cref="IServiceProvider"/> to retrieve the services from.</param>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>An enumeration of services of type <paramref name="serviceType"/>.</returns>
        public static IEnumerable<object> GetServices(this IServiceProvider provider, Type serviceType)
        {
            Condition.Requires(provider, nameof(provider)).IsNotNull();
            Condition.Requires(serviceType, nameof(serviceType)).IsNotNull();
            var enumerableServiceType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            return (IEnumerable<object>)provider.GetService(enumerableServiceType) ?? Enumerable.Empty<object>();
        }

        #endregion Methods

    }
}
