﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DDD.Core.Infrastructure.Data {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SqlServerScripts {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SqlServerScripts() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DDD.Core.Infrastructure.Data.SqlServerScripts", typeof(SqlServerScripts).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /****** Object:  Database [Test]    Script Date: 03/03/2022 10:29:20 ******/
        ///USE [master]
        ///GO
        ///IF EXISTS(SELECT * FROM [sys].[databases] where [name] = &apos;Test&apos;)
        ///begin
        ///ALTER DATABASE [Test] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
        ///DROP DATABASE [Test]
        ///end
        ///GO
        ///CREATE DATABASE [Test]
        /// CONTAINMENT = NONE
        /// ON  PRIMARY 
        ///( NAME = N&apos;Test&apos;, FILENAME = N&apos;C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Test.mdf&apos; , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
        /// LOG ON 
        ///( NAME = [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CreateDatabase {
            get {
                return ResourceManager.GetString("CreateDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USE [Test]
        ///GO
        ///EXEC spClearDatabase
        ///GO.
        /// </summary>
        internal static string ExcludeFailedEventStream {
            get {
                return ResourceManager.GetString("ExcludeFailedEventStream", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USE [Test]
        ///GO
        ///EXEC spClearDatabase
        ///GO
        ///INSERT [dbo].[EventStream]([Type], [Source], [Position], [RetryMax], [RetryDelays], [BlockSize]) VALUES (&apos;Person&apos;, &apos;ID&apos;, &apos;f7df5bd0-8763-677e-7e6b-3a0044746810&apos;, 5, &apos;10,60,120/80&apos;, 50)
        ///GO																			   
        ///INSERT [dbo].[EventStream]([Type], [Source], [Position], [RetryMax], [RetryDelays], [BlockSize]) VALUES (&apos;MedicalProduct&apos;, &apos;OFR&apos;, &apos;00000000-0000-0000-0000-000000000000&apos;, 3, &apos;60&apos;, 100)
        ///GO																			   .
        /// </summary>
        internal static string FindEventStreams {
            get {
                return ResourceManager.GetString("FindEventStreams", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USE [Test]
        ///GO
        ///EXEC spClearDatabase
        ///GO
        ///INSERT [dbo].[FailedEventStream] ([StreamId], [StreamType], [StreamSource], [StreamPosition], [EventId], [EventType], [ExceptionTime], [ExceptionType], [ExceptionMessage], [ExceptionSource], [ExceptionInfo], [BaseExceptionType], [BaseExceptionMessage], [RetryCount], [RetryMax], [RetryDelays], [BlockSize]) VALUES (N&apos;2&apos;, N&apos;MessageBox&apos;, N&apos;COL&apos;, N&apos;0a77707a-c147-9e1b-883a-08da0e368663&apos;, N&apos;e10add4d-1851-7ede-883b-08da0e368663&apos;, N&apos;DDD.Collaboration.Domain.Messages.MessageB [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string FindFailedEventStreams {
            get {
                return ResourceManager.GetString("FindFailedEventStreams", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USE [Test]
        ///GO
        ///EXEC spClearDatabase
        ///GO
        ///INSERT INTO [dbo].[Command] ([CommandId], [CommandType], [Body], [BodyFormat], [RecurringExpression], [LastExecutionTime], [LastExecutionStatus], [LastExceptionInfo])
        ///VALUES (N&apos;36beb37d-1e01-bb7d-fb2a-3a0044745345&apos;, N&apos;DDD.Core.Application.FakeCommand1, DDD.Core.Messages&apos;, &apos;{&quot;Property1&quot;:&quot;dummy&quot;,&quot;Property2&quot;:10}&apos;, N&apos;JSON&apos;, N&apos;* * * * *&apos;, NULL, NULL, NULL)
        ///GO
        ///INSERT INTO [dbo].[Command] ([CommandId], [CommandType], [Body], [BodyFormat], [RecurringExpression], [LastExe [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string FindRecurringCommands {
            get {
                return ResourceManager.GetString("FindRecurringCommands", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USE [Test]
        ///GO
        ///EXEC spClearDatabase
        ///GO
        ///INSERT [dbo].[FailedEventStream] ([StreamId], [StreamType], [StreamSource], [StreamPosition], [EventId], [EventType], [ExceptionTime], [ExceptionType], [ExceptionMessage], [ExceptionSource], [ExceptionInfo], [BaseExceptionType], [BaseExceptionMessage], [RetryCount], [RetryMax], [RetryDelays], [BlockSize]) VALUES (N&apos;2&apos;, N&apos;MessageBox&apos;, N&apos;COL&apos;, N&apos;0a77707a-c147-9e1b-883a-08da0e368663&apos;, N&apos;e10add4d-1851-7ede-883b-08da0e368663&apos;, N&apos;DDD.Collaboration.Domain.Messages.MessageB [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string IncludeFailedEventStream {
            get {
                return ResourceManager.GetString("IncludeFailedEventStream", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USE [Test]
        ///GO
        ///EXEC spClearDatabase
        ///GO
        ///INSERT INTO [dbo].[Command] ([CommandId], [CommandType], [Body], [BodyFormat], [RecurringExpression], [LastExecutionTime], [LastExecutionStatus], [LastExceptionInfo])
        ///VALUES (N&apos;36beb37d-1e01-bb7d-fb2a-3a0044745345&apos;, N&apos;DDD.Core.Application.FakeCommand1, DDD.Core.Messages&apos;, &apos;{&quot;Property1&quot;:&quot;dummy&quot;,&quot;Property2&quot;:10}&apos;, N&apos;JSON&apos;, N&apos;* * * * *&apos;, NULL, NULL, NULL)
        ///GO
        ///INSERT INTO [dbo].[Command] ([CommandId], [CommandType], [Body], [BodyFormat], [RecurringExpression], [LastExe [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string MarkRecurringCommandAsFailed {
            get {
                return ResourceManager.GetString("MarkRecurringCommandAsFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USE [Test]
        ///GO
        ///EXEC spClearDatabase
        ///GO
        ///INSERT INTO [dbo].[Command] ([CommandId], [CommandType], [Body], [BodyFormat], [RecurringExpression], [LastExecutionTime], [LastExecutionStatus], [LastExceptionInfo])
        ///VALUES (N&apos;36beb37d-1e01-bb7d-fb2a-3a0044745345&apos;, N&apos;DDD.Core.Application.FakeCommand1, DDD.Core.Messages&apos;, &apos;{&quot;Property1&quot;:&quot;dummy&quot;,&quot;Property2&quot;:10}&apos;, N&apos;JSON&apos;, N&apos;* * * * *&apos;, CAST(N&apos;2022-01-01T00:00:00.0000000&apos; AS DateTime2), N&apos;F&apos;, N&apos;System.TimeoutException: The operation has timed-out.&apos;)
        ///GO
        ///INSERT INTO [ [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string MarkRecurringCommandAsSuccessful {
            get {
                return ResourceManager.GetString("MarkRecurringCommandAsSuccessful", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USE [Test]
        ///GO
        ///EXEC spClearDatabase
        ///GO
        ///INSERT INTO TableWithId (Id) VALUES (1)
        ///GO
        ///INSERT INTO TableWithId (Id) VALUES (2)
        ///GO
        ///INSERT INTO TableWithId (Id) VALUES (3)
        ///GO
        ///INSERT INTO TableWithId (Id) VALUES (4)
        ///GO.
        /// </summary>
        internal static string NextId_ExistingRows {
            get {
                return ResourceManager.GetString("NextId_ExistingRows", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USE [Test]
        ///GO
        ///EXEC spClearDatabase
        ///GO																		   .
        /// </summary>
        internal static string NextId_NoRow {
            get {
                return ResourceManager.GetString("NextId_NoRow", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USE [Test]
        ///GO
        ///EXEC spClearDatabase
        ///GO
        ///INSERT [dbo].[Event] ([EventId], [EventType], [OccurredOn], [Body], [BodyFormat], [StreamId], [StreamType], [IssuedBy]) VALUES (N&apos;36beb37d-1e01-bb7d-fb2a-3a0044745345&apos;, N&apos;DDD.Collaboration.Domain.Messages.MessageBoxCreated, DDD.Collaboration.Messages&apos;, CAST(N&apos;2021-11-18T10:01:23.5280000&apos; AS DateTime2), N&apos;{&quot;boxId&quot;:2,&quot;occurredOn&quot;:&quot;2021-11-18T10:01:23.5277314+01:00&quot;}&apos;, N&apos;JSON&apos;, N&apos;2&apos;, N&apos;MessageBox&apos;, N&apos;Dr Maboul&apos;)
        ///GO
        ///INSERT [dbo].[Event] ([EventId], [EventType], [Occur [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ReadEventStream {
            get {
                return ResourceManager.GetString("ReadEventStream", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USE [Test]
        ///GO
        ///EXEC spClearDatabase
        ///GO
        ///INSERT [dbo].[Event] ([EventId], [EventType], [OccurredOn], [Body], [BodyFormat], [StreamId], [StreamType], [IssuedBy]) VALUES (N&apos;36beb37d-1e01-bb7d-fb2a-3a0044745345&apos;, N&apos;DDD.Collaboration.Domain.Messages.MessageBoxCreated, DDD.Collaboration.Messages&apos;, CAST(N&apos;2021-11-18T10:01:23.5280000&apos; AS DateTime2), N&apos;{&quot;boxId&quot;:2,&quot;occurredOn&quot;:&quot;2021-11-18T10:01:23.5277314+01:00&quot;}&apos;, N&apos;JSON&apos;, N&apos;2&apos;, N&apos;MessageBox&apos;, N&apos;Dr Maboul&apos;)
        ///GO
        ///INSERT [dbo].[Event] ([EventId], [EventType], [Occur [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ReadFailedEventStream {
            get {
                return ResourceManager.GetString("ReadFailedEventStream", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USE [Test]
        ///GO
        ///EXEC spClearDatabase
        ///GO.
        /// </summary>
        internal static string RegisterRecurringCommand {
            get {
                return ResourceManager.GetString("RegisterRecurringCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USE [Test]
        ///GO
        ///EXEC spClearDatabase
        ///GO.
        /// </summary>
        internal static string SubscribeToEventStream {
            get {
                return ResourceManager.GetString("SubscribeToEventStream", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USE [Test]
        ///GO
        ///EXEC spClearDatabase
        ///GO																		   
        ///INSERT [dbo].[EventStream]([Type], [Source], [Position], [RetryMax], [RetryDelays], [BlockSize]) VALUES (&apos;Person&apos;, &apos;ID&apos;, &apos;f7df5bd0-8763-677e-7e6b-3a0044746810&apos;, 5, &apos;10,60,120/80&apos;, 50)
        ///GO																			   
        ///INSERT [dbo].[EventStream]([Type], [Source], [Position], [RetryMax], [RetryDelays], [BlockSize]) VALUES (&apos;MedicalProduct&apos;, &apos;OFR&apos;, &apos;00000000-0000-0000-0000-000000000000&apos;, 3, &apos;60&apos;, 100)
        ///GO																			   .
        /// </summary>
        internal static string UpdateEventStreamPosition {
            get {
                return ResourceManager.GetString("UpdateEventStreamPosition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USE [Test]
        ///GO
        ///EXEC spClearDatabase
        ///GO
        ///INSERT [dbo].[FailedEventStream] ([StreamId], [StreamType], [StreamSource], [StreamPosition], [EventId], [EventType], [ExceptionTime], [ExceptionType], [ExceptionMessage], [ExceptionSource], [ExceptionInfo], [BaseExceptionType], [BaseExceptionMessage], [RetryCount], [RetryMax], [RetryDelays], [BlockSize]) VALUES (N&apos;2&apos;, N&apos;MessageBox&apos;, N&apos;COL&apos;, N&apos;0a77707a-c147-9e1b-883a-08da0e368663&apos;, N&apos;e10add4d-1851-7ede-883b-08da0e368663&apos;, N&apos;DDD.Collaboration.Domain.Messages.MessageB [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string UpdateFailedEventStream {
            get {
                return ResourceManager.GetString("UpdateFailedEventStream", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USE [Test]
        ///GO
        ///EXEC spClearDatabase
        ///GO
        ///INSERT [dbo].[FailedEventStream] ([StreamId], [StreamType], [StreamSource], [StreamPosition], [EventId], [EventType], [ExceptionTime], [ExceptionType], [ExceptionMessage], [ExceptionSource], [ExceptionInfo], [BaseExceptionType], [BaseExceptionMessage], [RetryCount], [RetryMax], [RetryDelays], [BlockSize]) VALUES (N&apos;2&apos;, N&apos;MessageBox&apos;, N&apos;COL&apos;, N&apos;0a77707a-c147-9e1b-883a-08da0e368663&apos;, N&apos;e10add4d-1851-7ede-883b-08da0e368663&apos;, N&apos;Xperthis.Collaboration.Domain.Messages.Mes [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string UpdateFailedEventStreamPosition {
            get {
                return ResourceManager.GetString("UpdateFailedEventStreamPosition", resourceCulture);
            }
        }
    }
}
