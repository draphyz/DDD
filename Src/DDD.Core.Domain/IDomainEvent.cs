using System;

namespace DDD.Core.Domain
{
    /// <summary>
    /// An occurrence of something that happened in the domain layer.
    /// </summary>
    public interface IDomainEvent
    {
        #region Properties

        DateTime OccurredOn { get; }

        #endregion Properties
    }
}