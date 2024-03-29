UPDATE FailedEventStream
SET    StreamPosition = @StreamPosition,
       EventId = @EventId,
       EventType = @EventType,
       ExceptionTime = @ExceptionTime,
       ExceptionType = @ExceptionType,
       ExceptionMessage = @ExceptionMessage,
       ExceptionSource = @ExceptionSource,
       ExceptionInfo = @ExceptionInfo,
       BaseExceptionType = @BaseExceptionType,
       BaseExceptionMessage = @BaseExceptionMessage,
       RetryCount = @RetryCount,
       RetryMax = @RetryMax,
       RetryDelays = @RetryDelays
WHERE  StreamId = @StreamId
AND    StreamType = @StreamType
AND    StreamSource = @StreamSource