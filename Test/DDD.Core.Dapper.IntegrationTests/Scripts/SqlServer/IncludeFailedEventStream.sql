USE [Test]
GO
EXEC spClearDatabase
GO
INSERT [dbo].[FailedEventStream] ([StreamId], [StreamType], [StreamSource], [StreamPosition], [EventId], [EventType], [ExceptionTime], [ExceptionType], [ExceptionMessage], [ExceptionSource], [ExceptionInfo], [BaseExceptionType], [BaseExceptionMessage], [RetryCount], [RetryMax], [RetryDelays], [BlockSize]) VALUES (N'2', N'MessageBox', N'COL', N'0a77707a-c147-9e1b-883a-08da0e368663', N'e10add4d-1851-7ede-883b-08da0e368663', N'DDD.Collaboration.Domain.Messages.MessageBoxCreated, DDD.Collaboration.Messages', CAST(N'2021-11-19T00:00:00' AS SmallDateTime), N'System.Exception, mscorlib', N'Invalid event', N'DDD.IdentityManagement, DDD.IdentityManagement.Application.MessageBoxCreatedEventHandler, Void Handle()', N'System.Exception: Invalid event ---> System.Exception: Format not supported.
   --- End of inner exception stack trace ---', N'System.Exception, mscorlib', N'Format not supported.', 0, 5, N'10,60,360', 100)
GO