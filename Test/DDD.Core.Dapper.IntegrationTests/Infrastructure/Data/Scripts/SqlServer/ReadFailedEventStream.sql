USE [Test]
GO
EXEC spClearDatabase
GO
INSERT [dbo].[Event] ([EventId], [EventType], [OccurredOn], [Body], [BodyFormat], [StreamId], [StreamType], [IssuedBy]) VALUES (N'36beb37d-1e01-bb7d-fb2a-3a0044745345', N'DDD.Collaboration.Domain.Messages.MessageBoxCreated, DDD.Collaboration.Messages', CAST(N'2021-11-18T10:01:23.5280000' AS DateTime2), N'{"boxId":2,"occurredOn":"2021-11-18T10:01:23.5277314+01:00"}', N'JSON', N'2', N'MessageBox', N'Dr Maboul')
GO
INSERT [dbo].[Event] ([EventId], [EventType], [OccurredOn], [Body], [BodyFormat], [StreamId], [StreamType], [IssuedBy]) VALUES (N'f7df5bd0-8763-677e-7e6b-3a0044746810', N'DDD.Collaboration.Domain.Messages.MessageDelivered, DDD.Collaboration.Messages', CAST(N'2021-11-05T00:00:00.0000000' AS DateTime2), N'{"messageId":1,"occurredOn":"2021-11-05T00:00:00+01:00"}', N'JSON', N'1', N'Message', N'Dr Maboul')
GO
INSERT [dbo].[Event] ([EventId], [EventType], [OccurredOn], [Body], [BodyFormat], [StreamId], [StreamType], [IssuedBy]) VALUES (N'1540563b-feb1-c526-2bb9-3a0044746811', N'DDD.Collaboration.Domain.Messages.MessageReceived, DDD.Collaboration.Messages', CAST(N'2021-11-18T10:01:30.4670000' AS DateTime2), N'{"messageId":1,"occurredOn":"2021-11-18T10:01:30.4670777+01:00"}', N'JSON', N'1', N'Message', N'Dr Maboul')
GO
INSERT [dbo].[Event] ([EventId], [EventType], [OccurredOn], [Body], [BodyFormat], [StreamId], [StreamType], [IssuedBy]) VALUES (N'bc90c871-e8ba-3d34-b6d2-3a0044746895', N'DDD.Collaboration.Domain.Messages.MessageDelivered, DDD.Collaboration.Messages', CAST(N'2021-11-09T00:00:00.0000000' AS DateTime2), N'{"messageId":2,"occurredOn":"2021-11-09T00:00:00+01:00"}', N'JSON', N'2', N'Message', N'Dr Maboul')
GO
INSERT [dbo].[Event] ([EventId], [EventType], [OccurredOn], [Body], [BodyFormat], [StreamId], [StreamType], [IssuedBy]) VALUES (N'810fc97b-1940-ebce-b8b9-3a0044746896', N'DDD.Collaboration.Domain.Messages.MessageReceived, DDD.Collaboration.Messages', CAST(N'2021-11-18T10:01:30.6420000' AS DateTime2), N'{"messageId":2,"occurredOn":"2021-11-18T10:01:30.6420787+01:00"}', N'JSON', N'2', N'Message', N'Dr Maboul')
GO
INSERT [dbo].[Event] ([EventId], [EventType], [OccurredOn], [Body], [BodyFormat], [StreamId], [StreamType], [IssuedBy]) VALUES (N'6481377b-cb6e-9106-cb43-3a00447468f9', N'DDD.Collaboration.Domain.Messages.MessageDelivered, DDD.Collaboration.Messages', CAST(N'2021-11-16T00:00:00.0000000' AS DateTime2), N'{"messageId":3,"occurredOn":"2021-11-16T00:00:00+01:00"}', N'JSON', N'3', N'Message', N'Dr Maboul')
GO
INSERT [dbo].[Event] ([EventId], [EventType], [OccurredOn], [Body], [BodyFormat], [StreamId], [StreamType], [IssuedBy]) VALUES (N'6b1d1c18-bfc2-2de3-d97c-3a00447468fa', N'DDD.Collaboration.Domain.Messages.MessageReceived, DDD.Collaboration.Messages', CAST(N'2021-11-18T10:01:30.7420000' AS DateTime2), N'{"messageId":3,"occurredOn":"2021-11-18T10:01:30.7417655+01:00"}', N'JSON', N'3', N'Message', N'Dr Maboul')
GO
INSERT [dbo].[Event] ([EventId], [EventType], [OccurredOn], [Body], [BodyFormat], [StreamId], [StreamType], [IssuedBy]) VALUES (N'097d449e-539e-bdf1-b50d-3a0184ad3555', N'DDD.Collaboration.Domain.Messages.MessageBoxCreated, DDD.Collaboration.Messages', CAST(N'2022-01-19T14:22:00.3680000' AS DateTime2), N'{"boxId":3,"occurredOn":"2022-01-19T14:22:00.3683143+01:00"}', N'JSON', N'3', N'MessageBox', N'Dr Folamour')
GO
INSERT [dbo].[Event] ([EventId], [EventType], [OccurredOn], [Body], [BodyFormat], [StreamId], [StreamType], [IssuedBy]) VALUES (N'a37b860d-63dc-6f86-c905-3a0188c3a52b', N'DDD.Collaboration.Domain.Messages.MessageCreated, DDD.Collaboration.Messages', CAST(N'2022-01-20T09:24:59.6050000' AS DateTime2), N'{"messageId":4,"occurredOn":"2022-01-20T09:24:59.6050066+01:00"}', N'JSON', N'4', N'Message', N'Dr Folamour')
GO
INSERT [dbo].[Event] ([EventId], [EventType], [OccurredOn], [Body], [BodyFormat], [StreamId], [StreamType], [IssuedBy]) VALUES (N'c997275a-12c8-7e95-50c7-3a0188c3b721', N'DDD.Collaboration.Domain.Messages.MessageSent, DDD.Collaboration.Messages', CAST(N'2022-01-20T09:25:06.0770000' AS DateTime2), N'{"messageId":4,"occurredOn":"2022-01-20T09:25:06.077499+01:00"}', N'JSON', N'4', N'Message', N'Dr Folamour')
GO
INSERT [dbo].[Event] ([EventId], [EventType], [OccurredOn], [Body], [BodyFormat], [StreamId], [StreamType], [IssuedBy]) VALUES (N'b7ef31c3-9453-3943-014a-3a0189bff875', N'DDD.Collaboration.Domain.Messages.MessageDelivered, DDD.Collaboration.Messages', CAST(N'2021-11-18T00:00:00.0000000' AS DateTime2), N'{"messageId":5,"occurredOn":"2021-11-18T00:00:00+01:00"}', N'JSON', N'5', N'Message', N'Dr Folamour')
GO
INSERT [dbo].[Event] ([EventId], [EventType], [OccurredOn], [Body], [BodyFormat], [StreamId], [StreamType], [IssuedBy]) VALUES (N'769621a1-8823-897c-04c3-3a0189bff876', N'DDD.Collaboration.Domain.Messages.MessageReceived, DDD.Collaboration.Messages', CAST(N'2022-01-20T14:00:33.6170000' AS DateTime2), N'{"messageId":5,"occurredOn":"2022-01-20T14:00:33.6168472+01:00"}', N'JSON', N'5', N'Message', N'Dr Folamour')
GO
INSERT [dbo].[Event] ([EventId], [EventType], [OccurredOn], [Body], [BodyFormat], [StreamId], [StreamType], [IssuedBy]) VALUES (N'321ea720-affd-6c91-3782-3a0189c0f051', N'DDD.Collaboration.Domain.Messages.MessageRead, DDD.Collaboration.Messages', CAST(N'2022-01-20T14:01:41.3250000' AS DateTime2), N'{"messageId":5,"occurredOn":"2022-01-20T14:01:41.3251974+01:00"}', N'JSON', N'5', N'Message', N'Dr Folamour')
GO
INSERT [dbo].[Event] ([EventId], [EventType], [OccurredOn], [Body], [BodyFormat], [StreamId], [StreamType], [IssuedBy]) VALUES (N'a224a074-c1d9-6c6f-0adc-3a0189c10ccc', N'DDD.Collaboration.Domain.Messages.MessageSentToBin, DDD.Collaboration.Messages', CAST(N'2022-01-20T14:01:48.6160000' AS DateTime2), N'{"messageId":5,"source":"Inbox","destination":"Binbox","occurredOn":"2022-01-20T14:01:48.6157105+01:00"}', N'JSON', N'5', N'Message', N'Dr Folamour')
GO
INSERT [dbo].[Event] ([EventId], [EventType], [OccurredOn], [Body], [BodyFormat], [StreamId], [StreamType], [IssuedBy]) VALUES (N'0096f748-41f4-2e2b-87f3-3a0189c1505f', N'DDD.Collaboration.Domain.Messages.MessageDeleted, DDD.Collaboration.Messages', CAST(N'2022-01-20T14:02:05.9150000' AS DateTime2), N'{"messageId":5,"occurredOn":"2022-01-20T14:02:05.9149942+01:00"}', N'JSON', N'5', N'Message', N'Dr Folamour')
GO
INSERT [dbo].[Event] ([EventId], [EventType], [OccurredOn], [Body], [BodyFormat], [StreamId], [StreamType], [IssuedBy]) VALUES (N'448cd6f8-6f90-a614-9dca-3a02639e313b', N'DDD.HealthcareDelivery.Domain.Prescriptions.PharmaceuticalPrescriptionCreated, DDD.HealthcareDelivery.Messages', CAST(N'2022-03-03T21:20:54.7930000' AS DateTime2), N'{"prescriptionId":1458,"occurredOn":"2022-03-03T21:20:54.7929687+01:00"}', N'JSON', N'1458', N'PharmaceuticalPrescription', N'Dr Folamour')
GO
