using System;

namespace DDD
{
    /// <summary>
    /// Defines a method that provides timestamps.
    /// </summary>
    public interface ITimestampProvider
    {

        #region Methods

        DateTime GetTimestamp();

        #endregion Methods
    }
}