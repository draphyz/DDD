using Conditions;
using System;

namespace DDD.Core
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

        #endregion Methods

    }
}
