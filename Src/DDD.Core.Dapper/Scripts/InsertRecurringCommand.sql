INSERT INTO Command
           (CommandId,
            CommandType,
            Body,
            BodyFormat,
            RecurringExpression)
     VALUES
           (@CommandId,
            @CommandType,
            @Body,
            @BodyFormat,
            @RecurringExpression)