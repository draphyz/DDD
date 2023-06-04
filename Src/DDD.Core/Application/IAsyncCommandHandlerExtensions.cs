using EnsureThat;
using System.Threading.Tasks;

namespace DDD.Core.Application
{  
    public static class IAsyncCommandHandlerExtensions
    {

        #region Methods

        public static Task HandleAsync<TCommand>(this IAsyncCommandHandler<TCommand> handler,
                                                TCommand command)
            where TCommand : class, ICommand

        {
            Ensure.That(handler, nameof(handler)).IsNotNull();
            return handler.HandleAsync(command, new MessageContext());
        }

        public static Task HandleAsync<TCommand>(this IAsyncCommandHandler<TCommand> handler, 
                                                 TCommand command, 
                                                 object context)
            where TCommand : class, ICommand

        {
            Ensure.That(handler, nameof(handler)).IsNotNull();
            return handler.HandleAsync(command, MessageContext.FromObject(context));
        }

        #endregion Methods

    }
}
