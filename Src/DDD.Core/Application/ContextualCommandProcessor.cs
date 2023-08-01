using EnsureThat;
using System;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// The command processor for processing generic commands in a specific bounded context. 
    /// </summary>
    public class ContextualCommandProcessor<TContext> : IContextualCommandProcessor<TContext>
        where TContext : BoundedContext
    {

        #region Fields

        private readonly IServiceProvider serviceProvider;

        #endregion Fields

        #region Constructors

        public ContextualCommandProcessor(IServiceProvider serviceProvider, TContext context)
        {
            Ensure.That(serviceProvider, nameof(serviceProvider)).IsNotNull();
            Ensure.That(context, nameof(context)).IsNotNull();
            this.serviceProvider = serviceProvider;
            this.Context = context;
        }

        #endregion Constructors

        #region Properties

        public TContext Context { get; }

        BoundedContext IContextualCommandProcessor.Context => this.Context;

        #endregion Properties

        #region Methods

        public void Process<TCommand>(TCommand command, IMessageContext context) where TCommand : class, ICommand
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            var handler = this.serviceProvider.GetService<ISyncCommandHandler<TCommand, TContext>>();
            if (handler == null) throw new InvalidOperationException($"The command handler for type {typeof(ISyncCommandHandler<TCommand, TContext>)} could not be found.");
            handler.Handle(command, context);
        }

        public void Process(ICommand command, IMessageContext context)
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            var handlerType = typeof(ISyncCommandHandler<,>).MakeGenericType(command.GetType(), this.Context.GetType());
            dynamic handler = this.serviceProvider.GetService(handlerType);
            if (handler == null) throw new InvalidOperationException($"The command handler for type {handlerType} could not be found.");
            handler.Handle((dynamic)command, context);
        }

        public Task ProcessAsync<TCommand>(TCommand command, IMessageContext context) where TCommand : class, ICommand
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            var handler = this.serviceProvider.GetService<IAsyncCommandHandler<TCommand, TContext>>();
            if (handler == null) throw new InvalidOperationException($"The command handler for type {typeof(IAsyncCommandHandler<TCommand, TContext>)} could not be found.");
            return handler.HandleAsync(command, context);
        }

        public Task ProcessAsync(ICommand command, IMessageContext context)
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            var handlerType = typeof(IAsyncCommandHandler<,>).MakeGenericType(command.GetType(), this.Context.GetType());
            dynamic handler = this.serviceProvider.GetService(handlerType);
            if (handler == null) throw new InvalidOperationException($"The command handler for type {handlerType} could not be found.");
            return handler.HandleAsync((dynamic)command, context);
        }

        #endregion Methods

    }
}