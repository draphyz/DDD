SELECT CommandId,
       CommandType,
       Body,
       BodyFormat,
       RecurringExpression
FROM Command
WHERE CommandType = @CommandType