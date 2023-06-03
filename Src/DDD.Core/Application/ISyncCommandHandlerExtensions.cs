using EnsureThat;

namespace DDD.Core.Application
{
    public static class ISyncCommandHandlerExtensions
    {

        #region Methods

        public static void Handle<TCommand>(this ISyncCommandHandler<TCommand> handler,
                                            TCommand command)
            where TCommand : class, ICommand

        {
            Ensure.That(handler, nameof(handler)).IsNotNull();
            handler.Handle(command, new MessageContext());
        }

        public static void Handle<TCommand>(this ISyncCommandHandler<TCommand> handler, 
                                            TCommand command, 
                                            object context)
            where TCommand : class, ICommand

        {
            Ensure.That(handler, nameof(handler)).IsNotNull();
            handler.Handle(command, MessageContext.FromObject(context));
        }

        #endregion Methods

    }
}
