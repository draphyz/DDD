UPDATE EventStream
SET Position = @Position
WHERE Type = @Type
AND Source = @Source