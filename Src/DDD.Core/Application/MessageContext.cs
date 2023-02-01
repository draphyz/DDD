using System.Collections.Generic;
using System.Threading;

namespace DDD.Core.Application
{
    using Collections;

    /// <summary>
    /// Provides access to the execution context of a message.
    /// </summary>
    public class MessageContext
        : Dictionary<string, object>, IMessageContext
    {

        #region Constructors

        public MessageContext()
        {
        }

        public MessageContext(IDictionary<string, object> dictionary) : base(dictionary)
        {
        }

        #endregion Constructors

        #region Methods

        public static IMessageContext CancellableContext(CancellationToken cancellationToken)
        {
            var context = new MessageContext();
            context.AddCancellationToken(cancellationToken);
            return context;
        }

        public static IMessageContext FromObject(object context)
        {
            MessageContext messageContext;
            if (context == null)
                messageContext = null;
            else
            {
                messageContext = new MessageContext();
                messageContext.AddObject(context);
            }
            return messageContext;
        }

        #endregion Methods

    }
}