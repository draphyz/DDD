UPDATE FailedEventStream
SET    StreamPosition = @Position
WHERE  StreamId = @Id
AND    StreamType = @Type
AND    StreamSource = @Source