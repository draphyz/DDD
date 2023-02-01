using System;

namespace DDD.Core.Application
{
    /// <summary>
    /// Encapsulates all information needed to generate an identifier for a recurring command.
    /// </summary>
    public class GenerateRecurringCommandId : IQuery<Guid>
    {
    }
}
