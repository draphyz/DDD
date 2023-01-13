﻿using Conditions;

namespace DDD.Core.Application
{
    using Collections;

    public static class ISyncCommandHandlerExtensions
    {

        #region Methods

        public static void Handle<TCommand>(this ISyncCommandHandler<TCommand> handler, 
                                            TCommand command, 
                                            object context)
            where TCommand : class, ICommand

        {
            Condition.Requires(handler, nameof(handler)).IsNotNull();
            handler.Handle(command, MessageContext.FromObject(context));
        }

        #endregion Methods

    }
}