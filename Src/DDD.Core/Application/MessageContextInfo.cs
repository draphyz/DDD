namespace DDD.Core.Application
{
    /// <summary>
    /// Standard contextual information about the message.
    /// </summary>
    public class MessageContextInfo
    {
        #region Fields

        public const string Event = "Event",
                            CancellationToken = "CancellationToken",
                            Stream = "Stream",
                            FailedStream = "FailedStream";

        #endregion Fields
    }
}
