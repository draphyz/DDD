SELECT   StreamId,          
         StreamType,        
         StreamSource,      
         StreamPosition,
         EventId,
         ExceptionTime,
         RetryCount,
         RetryMax,
         RetryDelays,
         BlockSize
FROM	 FailedEventStream
ORDER BY StreamSource, StreamType, StreamId