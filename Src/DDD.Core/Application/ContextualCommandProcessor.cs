using Conditions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using DDD.Core.Domain;

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
            Condition.Requires(serviceProvider, nameof(serviceProvider)).IsNotNull();
            Condition.Requires(context, nameof(context)).IsNotNull();
            this.serviceProvider = serviceProvider;
            this.Context = context;
        }

        #endregion Constructors

        #region Properties

        public TContext Context { get; }

        BoundedContext IContextualCommandProcessor.Context => this.Context;

        #endregion Properties

        #region Methods

        public void Process<TCommand>(TCommand command, IMessageContext context = null) where TCommand : class, ICommand
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            var handler = this.serviceProvider.GetService<ISyncCommandHandler<TCommand, TContext>>();
            if (handler == null) throw new InvalidOperationException($"The command handler for type {typeof(ISyncCommandHandler<TCommand, TContext>)} could not be found.");
            handler.Handle(command, context);
        }

        public Task ProcessAsync<TCommand>(TCommand command, IMessageContext context = null) where TCommand : class, ICommand
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            var handler = this.serviceProvider.GetService<IAsyncCommandHandler<TCommand, TContext>>();
            if (handler == null) throw new InvalidOperationException($"The command handler for type {typeof(IAsyncCommandHandler<TCommand, TContext>)} could not be found.");
            return handler.HandleAsync(command, context);
        }

        #endregion Methods

    }
}