using Conditions;
using System.Threading.Tasks;

namespace DDD.Core.Application
{  
    public static class IAsyncCommandHandlerExtensions
    {

        #region Methods

        public static Task HandleAsync<TCommand>(this IAsyncCommandHandler<TCommand> handler, 
                                                 TCommand command, 
                                                 object context)
            where TCommand : class, ICommand

        {
            Condition.Requires(handler, nameof(handler)).IsNotNull();
            return handler.HandleAsync(command, MessageContext.FromObject(context));
        }

        public static async Task UpdateStreamPositionIfDefinedAsync(this IAsyncCommandHandler<UpdateEventStreamPosition> handler, IMessageContext context)
        {
            Condition.Requires(handler, nameof(handler)).IsNotNull();
            if (context != null)
            {
                var @event = context.Event();
                var stream = context.Stream();
                if (@event != null && stream != null)
                {
                    var command = new UpdateEventStreamPosition
                    {
                        Position = @event.EventId,
                        Type = stream.Type, 
                        Source = stream.Source
                    };
                    await handler.HandleAsync(command, context);
                    stream.Position = @event.EventId;
                }
            }
        }

        #endregion Methods

    }
}
