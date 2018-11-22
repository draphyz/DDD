using System;

namespace DDD.Core.Domain
{
    /// <summary>
    /// Represents an occurrence of something that happened.
    /// </summary>
    public interface IEvent : IMessage
    {
        #region Properties

        DateTime OccurredOn { get; }

        #endregion Properties
    }
}