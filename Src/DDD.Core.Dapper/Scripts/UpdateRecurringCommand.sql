UPDATE Command
SET Body = @Body,
    BodyFormat = @BodyFormat,
    RecurringExpression = @RecurringExpression
WHERE CommandId = @CommandId