using SimpleInjector;
using SimpleInjector.Lifestyles;
using EnsureThat;
using System;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Application;
    using Domain;

    /// <summary>
    /// A decorator that defines a scope around the synchronous execution of a command.
    /// </summary>
    public class ThreadScopedCommandHandler<TCommand, TContext> : ISyncCommandHandler<TCommand, TContext>
        where TCommand : class, ICommand
        where TContext : BoundedContext, new()
    {

        #region Fields

        private readonly Container container;
        private readonly TContext context;
        private readonly Func<ISyncCommandHandler<TCommand, TContext>> handlerProvider;

        #endregion Fields

        #region Constructors

        public ThreadScopedCommandHandler(Func<ISyncCommandHandler<TCommand, TContext>> handlerProvider, Container container)
        {
            Ensure.That(handlerProvider, nameof(handlerProvider)).IsNotNull();
            Ensure.That(container, nameof(container)).IsNotNull();
            this.handlerProvider = handlerProvider;
            this.container = container;
            this.context = new TContext();
        }

        #endregion Constructors

        #region Properties

        public TContext Context => this.context;

        #endregion Properties

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
