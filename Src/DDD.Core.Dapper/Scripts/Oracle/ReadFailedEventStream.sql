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
     WHERE	 EventId BETWEEN @EventIdMin AND @EventIdMax
     AND     StreamId = @StreamId
     AND     StreamType = @StreamType
     ORDER BY EventId)
WHERE ROWNUM <= @Top