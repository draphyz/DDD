using EnsureThat;
using System;
using System.Collections.Generic;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    public abstract class ExtendableRegistrationOptionsBuilder<TOptions, TContainer> 
        : FluentBuilder<TOptions>, IExtendableRegistrationOptionsBuilder<TOptions, TContainer>
        where TOptions : class
        where TContainer : class
    {
        #region Fields

        private readonly List<Action<TContainer>> extensions = new List<Action<TContainer>>();

        #endregion Fields

        #region Methods

        void IExtendableRegistrationOptionsBuilder<TOptions, TContainer>.AddExtension(Action<TContainer> extension)
        {
            Ensure.That(extension, nameof(extension)).IsNotNull();
            this.extensions.Add(extension);
        }

        void IExtendableRegistrationOptionsBuilder<TOptions, TContainer>.ApplyExtensions(TContainer container)
        {
            foreach (var extension in this.extensions)
                extension(container);
        }

        #endregion Methods
    }
}
