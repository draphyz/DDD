﻿using SimpleInjector;
using SimpleInjector.Lifestyles;
using EnsureThat;
using System;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Application;

    /// <summary>
    /// A decorator that defines a scope around the synchronous execution of a command.
    /// </summary>
    public class ThreadScopedCommandHandler<TCommand> : ISyncCommandHandler<TCommand>
        where TCommand : class, ICommand
    {

        #region Fields

        private readonly Container container;
        private readonly Func<ISyncCommandHandler<TCommand>> handlerProvider;

        #endregion Fields

        #region Constructors

        public ThreadScopedCommandHandler(Func<ISyncCommandHandler<TCommand>> handlerProvider, Container container)
        {
            Ensure.That(handlerProvider, nameof(handlerProvider)).IsNotNull();
            Ensure.That(container, nameof(container)).IsNotNull();
            this.handlerProvider = handlerProvider;
            this.container = container;
        }

        #endregion Constructors

        #region Methods

        public void Handle(TCommand command, IMessageContext context = null)
        {
            using (ThreadScopedLifestyle.BeginScope(container))
            {
                var handler = this.handlerProvider();
                handler.Handle(command, context);
            }
        }

        #endregion Methods

    }
}
