BEGIN
  SPCLEARSCHEMA();
END;
/
INSERT INTO TEST.COMMAND (COMMANDID, COMMANDTYPE, BODY, BODYFORMAT, RECURRINGEXPRESSION, RECURRINGEXPRESSIONFORMAT, LASTEXECUTIONTIME, LASTEXECUTIONSTATUS, LASTEXCEPTIONINFO)
VALUES (N'7db3be36011e7dbbfb2a3a0044745345', N'DDD.Core.Application.FakeCommand1, DDD.Core.Messages', '{"Property1":"dummy","Property2":10}', N'JSON', N'* * * * *', N'CRON', to_timestamp('01/01/22 00:00:00,000000000','DD/MM/RR HH24:MI:SSXFF'),'F','System.TimeoutException: The operation has timed-out.')
/
INSERT INTO TEST.COMMAND (COMMANDID, COMMANDTYPE, BODY, BODYFORMAT, RECURRINGEXPRESSION, RECURRINGEXPRESSIONFORMAT, LASTEXECUTIONTIME, LASTEXECUTIONSTATUS, LASTEXCEPTIONINFO)
VALUES (N'd05bdff763877e677e6b3a0044746810', N'DDD.Core.Application.FakeCommand2, DDD.Core.Messages', '{"Property1":"dummy","Property2":10,"Property3":"2022-12-24T14:49:44.361964+01:00"}', N'JSON', N'0  0 1 * *', N'CRON', NULL, NULL, NULL)
/