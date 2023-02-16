namespace DDD.Core.Application
{
    /// <summary>
    /// Standard contextual information about the message.
    /// </summary>
    public class MessageContextInfo
    {
        #region Fields

        public const string Event = nameof(Event),
                            CancellationToken = nameof(CancellationToken),
                            Stream = nameof(Stream),
                            FailedStream = nameof(FailedStream),
                            BoundedContext = nameof(BoundedContext);

        #endregion Fields
    }
}
