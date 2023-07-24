using System;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    public interface IExtendableRegistrationOptionsBuilder<TOptions, TContainer> : IObjectBuilder<TOptions>
        where TOptions: class
        where TContainer : class
    {

        #region Methods

        void AddExtension(Action<TContainer> extension);

        void ApplyExtensions(TContainer container);

        #endregion Methods

    }
}
