SELECT *
FROM 
    (SELECT  EventId,
             EventType,
             OccurredOn,
             Body,
             BodyFormat,
             StreamId,
             StreamType,
             IssuedBy
     FROM	 Event
     WHERE	 EventId > @StreamPosition
     AND     StreamId NOT IN @ExcludedStreamIds
     AND     StreamType = @StreamType
     ORDER BY EventId)
WHERE ROWNUM <= @Top