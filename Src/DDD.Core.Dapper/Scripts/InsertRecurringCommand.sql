INSERT INTO Command
           (CommandId,
            CommandType,
            Body,
            BodyFormat,
            RecurringExpression,
            RecurringExpressionFormat)
     VALUES
           (@CommandId,
            @CommandType,
            @Body,
            @BodyFormat,
            @RecurringExpression,
            @RecurringExpressionFormat)