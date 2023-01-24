USE [Test]
GO
EXEC spClearDatabase
GO																		   
INSERT [dbo].[EventStream]([Type], [Source], [Position], [RetryMax], [RetryDelays], [BlockSize]) VALUES ('Person', 'ID', 'f7df5bd0-8763-677e-7e6b-3a0044746810', 5, '10,60,120/80', 50)
GO																			   
INSERT [dbo].[EventStream]([Type], [Source], [Position], [RetryMax], [RetryDelays], [BlockSize]) VALUES ('MedicalProduct', 'OFR', '00000000-0000-0000-0000-000000000000', 3, '60', 100)
GO																			   