﻿using Polly;
using Conditions;

namespace DDD.Core.Infrastructure.ErrorHandling
{
    using Application;
    using DDD.Core.Domain;

    /// <summary>
    /// A decorator that applies a resilience policy to the synchronous execution of a command.
    /// </summary>
    public class SyncPollyCommandHandler<TCommand, TContext> : ISyncCommandHandler<TCommand, TContext>
        where TCommand : class, ICommand
        where TContext : BoundedContext
    {

        #region Fields

        private readonly ISyncCommandHandler<TCommand, TContext> handler;
        private readonly ISyncPolicy policy;

        #endregion Fields

        #region Constructors

        public SyncPollyCommandHandler(ISyncCommandHandler<TCommand, TContext> handler, ISyncPolicy policy)
        {
            Condition.Requires(handler, nameof(handler)).IsNotNull();
            Condition.Requires(policy, nameof(policy)).IsNotNull();
            this.handler = handler;
            this.policy = policy;
        }

        #endregion Constructors

        #region Properties

        public TContext Context => this.handler.Context;

        #endregion Properties

        #region Methods

        public void Handle(TCommand command, IMessageContext context = null)
        {
            policy.Execute(() => this.handler.Handle(command, context));
        }

        #endregion Methods

    }
}