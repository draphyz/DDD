﻿using EnsureThat;

namespace DDD.Core.Application
{
    public static class ISyncQueryHandlerExtensions
    {

        #region Methods

        public static TResult Handle<TQuery, TResult>(this ISyncQueryHandler<TQuery, TResult> handler, 
                                                      TQuery query, 
                                                      object context)
            where TQuery : class, IQuery<TResult>

        {
            Ensure.That(handler, nameof(handler)).IsNotNull();
            return handler.Handle(query, MessageContext.FromObject(context));
        }

        #endregion Methods

    }
}
