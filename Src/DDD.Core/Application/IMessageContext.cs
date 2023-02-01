using System.Collections.Generic;

namespace DDD.Core.Application
{
    /// <summary>
    /// Provides access to the execution context of a message.
    /// </summary>
    public interface IMessageContext : IDictionary<string, object>
    {
    }
}