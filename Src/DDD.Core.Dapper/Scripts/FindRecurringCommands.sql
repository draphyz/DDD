SELECT CommandId,
       CommandType,
       Body,
       BodyFormat,
       RecurringExpression,
       RecurringExpressionFormat
FROM Command
ORDER BY CommandId