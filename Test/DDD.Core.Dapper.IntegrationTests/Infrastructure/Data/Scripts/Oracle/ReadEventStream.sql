BEGIN
  SPCLEARSCHEMA();
END;
/
INSERT INTO TEST.Event (EventId, EventType, OccurredOn, Body, BodyFormat, StreamId, StreamType, IssuedBy) VALUES (N'08D9FDD90A9DE8B608025680BF57BB60', N'DDD.Collaboration.Domain.Messages.MessageBoxCreated, DDD.Collaboration.Messages', TO_TIMESTAMP(N'2021-11-18 10:01:23.5280000', 'YYYY-MM-DD HH24:MI:SS.FF7'), N'{"boxId":2,"occurredOn":"2021-11-18T10:01:23.5277314+01:00"}', N'JSON', N'2', N'MessageBox', N'Dr Maboul')
/
INSERT INTO TEST.Event (EventId, EventType, OccurredOn, Body, BodyFormat, StreamId, StreamType, IssuedBy) VALUES (N'08D9FDD90A9E0FBD9EC5AE08E4F34FF4', N'DDD.Collaboration.Domain.Messages.MessageDelivered, DDD.Collaboration.Messages', TO_TIMESTAMP(N'2021-11-05 00:00:00.0000000', 'YYYY-MM-DD HH24:MI:SS.FF7'), N'{"messageId":1,"occurredOn":"2021-11-05T00:00:00+01:00"}', N'JSON', N'1', N'Message', N'Dr Maboul')
/
INSERT INTO TEST.Event (EventId, EventType, OccurredOn, Body, BodyFormat, StreamId, StreamType, IssuedBy) VALUES (N'08D9FDD90A9E0FBEFD7C8DF7E92B9FE9', N'DDD.Collaboration.Domain.Messages.MessageReceived, DDD.Collaboration.Messages', TO_TIMESTAMP(N'2021-11-18 10:01:30.4670000', 'YYYY-MM-DD HH24:MI:SS.FF7'), N'{"messageId":1,"occurredOn":"2021-11-18T10:01:30.4670777+01:00"}', N'JSON', N'1', N'Message', N'Dr Maboul')
/
INSERT INTO TEST.Event (EventId, EventType, OccurredOn, Body, BodyFormat, StreamId, StreamType, IssuedBy) VALUES (N'08D9FDD90A9E0FBF1C913030E947036B', N'DDD.Collaboration.Domain.Messages.MessageDelivered, DDD.Collaboration.Messages', TO_TIMESTAMP(N'2021-11-09 00:00:00.0000000', 'YYYY-MM-DD HH24:MI:SS.FF7'), N'{"messageId":2,"occurredOn":"2021-11-09T00:00:00+01:00"}', N'JSON', N'2', N'Message', N'Dr Maboul')
/
INSERT INTO TEST.Event (EventId, EventType, OccurredOn, Body, BodyFormat, StreamId, StreamType, IssuedBy) VALUES (N'08D9FDD90A9E0FC054515E9FA7100251', N'DDD.Collaboration.Domain.Messages.MessageReceived, DDD.Collaboration.Messages', TO_TIMESTAMP(N'2021-11-18 10:01:30.6420000', 'YYYY-MM-DD HH24:MI:SS.FF7'), N'{"messageId":2,"occurredOn":"2021-11-18T10:01:30.6420787+01:00"}', N'JSON', N'2', N'Message', N'Dr Maboul')
/
INSERT INTO TEST.Event (EventId, EventType, OccurredOn, Body, BodyFormat, StreamId, StreamType, IssuedBy) VALUES (N'08D9FDD90A9E0FC143B339FD2DFC6AF7', N'DDD.Collaboration.Domain.Messages.MessageDelivered, DDD.Collaboration.Messages', TO_TIMESTAMP(N'2021-11-16 00:00:00.0000000', 'YYYY-MM-DD HH24:MI:SS.FF7'), N'{"messageId":3,"occurredOn":"2021-11-16T00:00:00+01:00"}', N'JSON', N'3', N'Message', N'Dr Maboul')
/
INSERT INTO TEST.Event (EventId, EventType, OccurredOn, Body, BodyFormat, StreamId, StreamType, IssuedBy) VALUES (N'08D9FDD90A9E0FC2F8E7B0809EB5DD00', N'DDD.Collaboration.Domain.Messages.MessageReceived, DDD.Collaboration.Messages', TO_TIMESTAMP(N'2021-11-18 10:01:30.7420000', 'YYYY-MM-DD HH24:MI:SS.FF7'), N'{"messageId":3,"occurredOn":"2021-11-18T10:01:30.7417655+01:00"}', N'JSON', N'3', N'Message', N'Dr Maboul')
/
INSERT INTO TEST.Event (EventId, EventType, OccurredOn, Body, BodyFormat, StreamId, StreamType, IssuedBy) VALUES (N'08D9FDD90A9E0FC3FE28F1DD2406A828', N'DDD.Collaboration.Domain.Messages.MessageBoxCreated, DDD.Collaboration.Messages', TO_TIMESTAMP(N'2022-01-19 14:22:00.3680000', 'YYYY-MM-DD HH24:MI:SS.FF7'), N'{"boxId":3,"occurredOn":"2022-01-19T14:22:00.3683143+01:00"}', N'JSON', N'3', N'MessageBox', N'Dr Folamour')
/
INSERT INTO TEST.Event (EventId, EventType, OccurredOn, Body, BodyFormat, StreamId, StreamType, IssuedBy) VALUES (N'08D9FDD90A9E0FC412249EE7F3177482', N'DDD.Collaboration.Domain.Messages.MessageCreated, DDD.Collaboration.Messages', TO_TIMESTAMP(N'2022-01-20 09:24:59.6050000', 'YYYY-MM-DD HH24:MI:SS.FF7'), N'{"messageId":4,"occurredOn":"2022-01-20T09:24:59.6050066+01:00"}', N'JSON', N'4', N'Message', N'Dr Folamour')
/
INSERT INTO TEST.Event (EventId, EventType, OccurredOn, Body, BodyFormat, StreamId, StreamType, IssuedBy) VALUES (N'08D9FDD90A9E0FC5D5B4F9DF4FA15CA6', N'DDD.Collaboration.Domain.Messages.MessageSent, DDD.Collaboration.Messages', TO_TIMESTAMP(N'2022-01-20 09:25:06.0770000', 'YYYY-MM-DD HH24:MI:SS.FF7'), N'{"messageId":4,"occurredOn":"2022-01-20T09:25:06.077499+01:00"}', N'JSON', N'4', N'Message', N'Dr Folamour')
/
INSERT INTO TEST.Event (EventId, EventType, OccurredOn, Body, BodyFormat, StreamId, StreamType, IssuedBy) VALUES (N'08D9FDD90A9E0FC68271ABEFDF727858', N'DDD.Collaboration.Domain.Messages.MessageDelivered, DDD.Collaboration.Messages', TO_TIMESTAMP(N'2021-11-18 00:00:00.0000000', 'YYYY-MM-DD HH24:MI:SS.FF7'), N'{"messageId":5,"occurredOn":"2021-11-18T00:00:00+01:00"}', N'JSON', N'5', N'Message', N'Dr Folamour')
/
INSERT INTO TEST.Event (EventId, EventType, OccurredOn, Body, BodyFormat, StreamId, StreamType, IssuedBy) VALUES (N'08D9FDD90A9E0FC707773E1979F9392B', N'DDD.Collaboration.Domain.Messages.MessageReceived, DDD.Collaboration.Messages', TO_TIMESTAMP(N'2022-01-20 14:00:33.6170000', 'YYYY-MM-DD HH24:MI:SS.FF7'), N'{"messageId":5,"occurredOn":"2022-01-20T14:00:33.6168472+01:00"}', N'JSON', N'5', N'Message', N'Dr Folamour')
/
INSERT INTO TEST.Event (EventId, EventType, OccurredOn, Body, BodyFormat, StreamId, StreamType, IssuedBy) VALUES (N'08D9FDD90A9E0FC8E72DE94A0F7D4902', N'DDD.Collaboration.Domain.Messages.MessageRead, DDD.Collaboration.Messages', TO_TIMESTAMP(N'2022-01-20 14:01:41.3250000', 'YYYY-MM-DD HH24:MI:SS.FF7'), N'{"messageId":5,"occurredOn":"2022-01-20T14:01:41.3251974+01:00"}', N'JSON', N'5', N'Message', N'Dr Folamour')
/
INSERT INTO TEST.Event (EventId, EventType, OccurredOn, Body, BodyFormat, StreamId, StreamType, IssuedBy) VALUES (N'08D9FDD90A9E0FC93CFE09C19F22F068', N'DDD.Collaboration.Domain.Messages.MessageSentToBin, DDD.Collaboration.Messages', TO_TIMESTAMP(N'2022-01-20 14:01:48.6160000', 'YYYY-MM-DD HH24:MI:SS.FF7'), N'{"messageId":5,"source":"Inbox","destination":"Binbox","occurredOn":"2022-01-20T14:01:48.6157105+01:00"}', N'JSON', N'5', N'Message', N'Dr Folamour')
/
INSERT INTO TEST.Event (EventId, EventType, OccurredOn, Body, BodyFormat, StreamId, StreamType, IssuedBy) VALUES (N'08D9FDD90A9E0FCA7C1B78288DB70EE6', N'DDD.Collaboration.Domain.Messages.MessageDeleted, DDD.Collaboration.Messages', TO_TIMESTAMP(N'2022-01-20 14:02:05.9150000', 'YYYY-MM-DD HH24:MI:SS.FF7'), N'{"messageId":5,"occurredOn":"2022-01-20T14:02:05.9149942+01:00"}', N'JSON', N'5', N'Message', N'Dr Folamour')
/
INSERT INTO TEST.Event (EventId, EventType, OccurredOn, Body, BodyFormat, StreamId, StreamType, IssuedBy) VALUES (N'08D9FDD90A9E0FCB9B08D443602F2815', N'DDD.HealthcareDelivery.Domain.Prescriptions.PharmaceuticalPrescriptionCreated, DDD.HealthcareDelivery.Messages', TO_TIMESTAMP(N'2022-03-03 21:20:54.7930000', 'YYYY-MM-DD HH24:MI:SS.FF7'), N'{"prescriptionId":1458,"occurredOn":"2022-03-03T21:20:54.7929687+01:00"}', N'JSON', N'1458', N'PharmaceuticalPrescription', N'Dr Folamour')
/
