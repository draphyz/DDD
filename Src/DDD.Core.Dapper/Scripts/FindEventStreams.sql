SELECT   Type,
         Source,
         Position,
         RetryMax,
         RetryDelays,
         BlockSize
FROM	 EventStream
ORDER BY Source, Type