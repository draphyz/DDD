/****** Object:  Database [Test]    Script Date: 03/03/2022 10:29:20 ******/
USE [master]
GO
IF EXISTS(SELECT * FROM [sys].[databases] where [name] = 'Test')
begin
ALTER DATABASE [Test] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
DROP DATABASE [Test]
end
GO
CREATE DATABASE [Test]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Test', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Test.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Test_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Test_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Test] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Test].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Test] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Test] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Test] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Test] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Test] SET ARITHABORT OFF 
GO
ALTER DATABASE [Test] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Test] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [Test] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Test] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Test] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Test] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Test] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Test] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Test] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Test] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Test] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Test] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Test] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Test] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Test] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Test] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Test] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Test] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Test] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Test] SET  MULTI_USER 
GO
ALTER DATABASE [Test] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Test] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Test] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Test] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [Test]
GO
/****** Object:  StoredProcedure [dbo].[spClearDatabase]    Script Date: 03/03/2022 10:29:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spClearDatabase]
AS
BEGIN
	SET NOCOUNT ON
	EXEC sys.sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'
	EXEC sys.sp_MSforeachtable 'DELETE FROM ?'
	EXEC sys.sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL'
	EXEC sys.sp_MSforeachtable 'IF OBJECTPROPERTY(object_id(''?''), ''TableHasIdentity'') = 1 DBCC CHECKIDENT (''?'', RESEED, 0)'
	DECLARE @restartseq nvarchar(max)
	DECLARE rcursor CURSOR 
	FOR 
	SELECT
	  'ALTER SEQUENCE '
	+  QUOTENAME(schema_name(schema_id))
	+  '.'
	+  QUOTENAME(name)
	+  ' RESTART'
	FROM sys.sequences
	OPEN rcursor
	FETCH NEXT FROM rcursor INTO @restartseq
	WHILE @@FETCH_STATUS = 0
	BEGIN 
		EXEC sp_executesql @restartseq
		FETCH NEXT FROM rcursor INTO @restartseq
	END
	CLOSE rcursor
	DEALLOCATE rcursor
	END

GO
/****** Object:  Table [dbo].[Command]    Script Date: 05/01/2023 13:42:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Command](
	[CommandId] [uniqueidentifier] NOT NULL,
	[CommandType] [varchar](250) NOT NULL,
	[Body] [varchar](max) NOT NULL,
	[BodyFormat] [varchar](20) NOT NULL,
	[RecurringExpression] [varchar](150) NOT NULL,
	[RecurringExpressionFormat] [varchar](20) NOT NULL,
	[LastExecutionTime] [datetime2](3) NULL,
	[LastExecutionStatus] [char](1) NULL,
	[LastExceptionInfo] [varchar](max) NULL,
 CONSTRAINT [PK_Command] PRIMARY KEY CLUSTERED 
(
	[CommandId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Command]    Script Date: 05/01/2023 13:42:07 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Command] ON [dbo].[Command]
(
	[CommandType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Event]    Script Date: 03/03/2022 10:29:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Event](
	[EventId] [uniqueidentifier] NOT NULL,
	[EventType] [varchar](250) NOT NULL,
	[OccurredOn] [datetime2](3) NOT NULL,
	[Body] [varchar](max) NOT NULL,
	[BodyFormat] [varchar](20) NOT NULL,
	[StreamId] [varchar](50) NOT NULL,
	[StreamType] [varchar](50) NOT NULL,
	[IssuedBy] [varchar](100) NULL,
 CONSTRAINT [PK_dbo.Event] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventStream]    Script Date: 07/03/2022 11:48:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventStream](
	[Type] [varchar](50) NOT NULL,
	[Source] [varchar](5) NOT NULL,
	[Position] [uniqueidentifier] NOT NULL,
	[RetryMax] [tinyint] NOT NULL,
	[RetryDelays] [varchar](50) NOT NULL,
	[BlockSize] [smallint] NOT NULL,
 CONSTRAINT [PK_EventStream] PRIMARY KEY CLUSTERED 
(
	[Type] ASC,
	[Source] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FailedEventStream]    Script Date: 15/03/2022 12:22:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FailedEventStream](
	[StreamId] [varchar](50) NOT NULL,
	[StreamType] [varchar](50) NOT NULL,
	[StreamSource] [varchar](5) NOT NULL,
	[StreamPosition] [uniqueidentifier] NOT NULL,
	[EventId] [uniqueidentifier] NOT NULL,
	[EventType] [varchar](250) NOT NULL,
	[ExceptionTime] [datetime2](3) NOT NULL,
	[ExceptionType] [varchar](250) NOT NULL,
	[ExceptionMessage] [varchar](max) NOT NULL,
	[ExceptionSource] [varchar](250) NOT NULL,
	[ExceptionInfo] [varchar](max) NOT NULL,
	[BaseExceptionType] [varchar](250) NOT NULL,
	[BaseExceptionMessage] [varchar](max) NOT NULL,
	[RetryCount] [tinyint] NOT NULL,
	[RetryMax] [tinyint] NOT NULL,
	[RetryDelays] [varchar](50) NULL,
	[BlockSize] [smallint] NOT NULL,
 CONSTRAINT [PK_FailedEventStream] PRIMARY KEY CLUSTERED 
(
	[StreamId] ASC,
	[StreamType] ASC,
	[StreamSource] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TableWithId]    Script Date: 03/05/2022 16:24:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TableWithId](
	[Id] [int] NOT NULL,
 CONSTRAINT [PK_TableWithId] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
USE [master]
GO
ALTER DATABASE [Test] SET  READ_WRITE 
GO