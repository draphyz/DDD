USE [Test]
GO
EXEC spClearDatabase
GO
INSERT INTO [dbo].[Command] ([CommandId], [CommandType], [Body], [BodyFormat], [RecurringExpression], [RecurringExpressionFormat], [LastExecutionTime], [LastExecutionStatus], [LastExceptionInfo])
VALUES (N'36beb37d-1e01-bb7d-fb2a-3a0044745345', N'DDD.Core.Application.FakeCommand1, DDD.Core.Messages', '{"Property1":"dummy","Property2":10}', N'JSON', N'* * * * *', N'CRON', NULL, NULL, NULL)
GO
INSERT INTO [dbo].[Command] ([CommandId], [CommandType], [Body], [BodyFormat], [RecurringExpression], [RecurringExpressionFormat], [LastExecutionTime], [LastExecutionStatus], [LastExceptionInfo])
VALUES (N'f7df5bd0-8763-677e-7e6b-3a0044746810', N'DDD.Core.Application.FakeCommand2, DDD.Core.Messages', '{"Property1":"dummy","Property2":10,"Property3":"2022-12-24T14:49:44.361964+01:00"}', N'JSON', N'0  0 1 * *', N'CRON', NULL, NULL, NULL)
GO