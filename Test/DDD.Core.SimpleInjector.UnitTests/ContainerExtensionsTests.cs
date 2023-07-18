using Xunit;
using FluentAssertions;
using SimpleInjector;
using System;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using System.Collections.Generic;
using DDD.Serialization;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Application;
    using Domain;
    using Mapping;
    using Data;

    public class ContainerExtensionsTests : IDisposable
    {

        #region Fields

        private readonly Container container;

        #endregion Fields

        #region Constructors

        public ContainerExtensionsTests()
        {
            this.container = new Container();
            this.container.Options.EnableAutoVerification = false;
        }

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
            this.container.Dispose();
        }

        [Fact]
        public void GetDecoratorChainOf_WithSimpleDecoratorsAndFromDecorated_ReturnsExpectedChain()
        {
            // Arrange
            this.container.Register<IDoSomething, Decorated>();
            this.container.RegisterDecorator<IDoSomething, FirstDecorator>();
            this.container.RegisterDecorator<IDoSomething, SecondDecorator>();
            var expectedChain = new []
            {
                new RegisteredType(typeof(Decorated), Lifestyle.Transient),
                new RegisteredType(typeof(FirstDecorator), Lifestyle.Transient),
                new RegisteredType(typeof(SecondDecorator), Lifestyle.Transient)
            };
            // Act
            var chain = this.container.GetDecoratorChainOf<IDoSomething>(fromDecorated:true);
            // Assert
            chain.Should().BeEquivalentTo(expectedChain);
        }

        [Fact]
        public void GetDecoratorChainOf_WithSimpleDecoratorsAndToDecorated_ReturnsExpectedChain()
        {
            // Arrange
            this.container.Register<IDoSomething, Decorated>();
            this.container.RegisterDecorator<IDoSomething, FirstDecorator>();
            this.container.RegisterDecorator<IDoSomething, SecondDecorator>();
            var expectedChain = new[]
            {
                new RegisteredType(typeof(SecondDecorator), Lifestyle.Transient),
                new RegisteredType(typeof(FirstDecorator), Lifestyle.Transient),
                new RegisteredType(typeof(Decorated), Lifestyle.Transient)
            };
            // Act
            var chain = this.container.GetDecoratorChainOf<IDoSomething>(fromDecorated:false);
            // Assert
            chain.Should().BeEquivalentTo(expectedChain);
        }

        [Fact]
        public void GetDecoratorChainOf_WithDelegatingDecoratorsAndFromDecorated_ReturnsExpectedChain()
        {
            // Arrange
            this.container.Register<IDoSomething, Decorated>();
            this.container.RegisterDecorator<IDoSomething, FirstDelegatingDecorator>(Lifestyle.Singleton);
            this.container.RegisterDecorator<IDoSomething, SecondDelegatingDecorator>(Lifestyle.Singleton);
            var expectedChain = new[]
            {
                new RegisteredType(typeof(Decorated), Lifestyle.Transient),
                new RegisteredType(typeof(FirstDelegatingDecorator), Lifestyle.Singleton),
                new RegisteredType(typeof(SecondDelegatingDecorator), Lifestyle.Singleton)
            };
            // Act
            var chain = this.container.GetDecoratorChainOf<IDoSomething>(fromDecorated: true);
            // Assert
            chain.Should().BeEquivalentTo(expectedChain);
        }

        [Fact]
        public void GetDecoratorChainOf_WithDelegatingDecoratorsAndToDecorated_ReturnsExpectedChain()
        {
            // Arrange
            this.container.Register<IDoSomething, Decorated>();
            this.container.RegisterDecorator<IDoSomething, FirstDelegatingDecorator>(Lifestyle.Singleton);
            this.container.RegisterDecorator<IDoSomething, SecondDelegatingDecorator>(Lifestyle.Singleton);
            var expectedChain = new[]
            {
                new RegisteredType(typeof(SecondDelegatingDecorator), Lifestyle.Singleton),
                new RegisteredType(typeof(FirstDelegatingDecorator), Lifestyle.Singleton),
                new RegisteredType(typeof(Decorated), Lifestyle.Transient)
            };
            // Act
            var chain = this.container.GetDecoratorChainOf<IDoSomething>(fromDecorated: false);
            // Assert
            chain.Should().BeEquivalentTo(expectedChain);
        }

        [Fact]
        public void ConfigureApp_WhithoutOptions_RegistersNullLoggerFactory()
        {
            // Act
            this.container.ConfigureApp(_ => { });
            // Assert
            var registration = container.GetRegistration<ILoggerFactory>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.Lifestyle == Lifestyle.Singleton);
            var loggerFactory = registration.GetInstance();
            loggerFactory.Should().BeOfType<NullLoggerFactory>();
        }

        [Fact]
        public void ConfigureApp_WhithoutOptions_RegistersServiceProvider()
        {
            // Act
            this.container.ConfigureApp(_ => { });
            // Assert
            var registration = container.GetRegistration<IServiceProvider>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.Lifestyle == Lifestyle.Singleton);
            var serviceProvider = registration.GetInstance();
            serviceProvider.Should().Be(container);
        }

        [Fact]
        public void ConfigureApp_WithConfigureDbConnectionFor_RegistersDbConnectionSettings()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.ConfigureDbConnectionFor<FakeContext>(oc =>
                {
                    oc.SetProviderName("FakeProviderName");
                    oc.SetConnectionString("FakeConnectionString");
                });
            });
            // Assert
            var registration = container.GetRegistration<DbConnectionSettings<FakeContext>>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.Lifestyle == Lifestyle.Singleton);
        }

        [Fact]
        public void ConfigureApp_WithConfigureDbConnectionFor_RegistersDbConnectionProvider()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.ConfigureDbConnectionFor<FakeContext>(oc =>
                {
                    oc.SetProviderName("FakeProviderName");
                    oc.SetConnectionString("FakeConnectionString");
                });
            });
            // Assert
            var registration = container.GetRegistration<IDbConnectionProvider<FakeContext>>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.ImplementationType == typeof(LazyDbConnectionProvider<FakeContext>)
                                                                            && r.Lifestyle == container.Options.DefaultScopedLifestyle);
        }

        [Fact]
        public void ConfigureApp_WithConfigureConsumerFor_RegistersEventConsumerAsSingleton()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.ConfigureEvents(oe =>
                {
                    oe.ConfigureConsumerFor<FakeContext>();
                });
            });
            // Assert
            var registration = container.GetRegistration<IEventConsumer<FakeContext>>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.ImplementationType == typeof(EventConsumer<FakeContext>)
                                                                            && r.Lifestyle == Lifestyle.Singleton);
        }

        [Fact]
        public void ConfigureApp_WithConfigureConsumerFor_RegistersEventConsumerInCollection()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.RegisterTypesFrom(Assembly.GetExecutingAssembly());
                o.RegisterSerializer(SerializationFormat.Json, () => Substitute.For<ITextSerializer>());
                o.ConfigureCommands();
                o.ConfigureQueries();
                o.ConfigureEvents(oe =>
                {
                    oe.ConfigureConsumerFor<FakeContext>();
                });
            });
            // Assert
            var registration = container.GetRegistration<IEnumerable<IEventConsumer>>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.Lifestyle == Lifestyle.Singleton);
            var consumers = (IEnumerable<IEventConsumer>)registration.GetInstance();
            consumers.Should().ContainSingle(c => c is IEventConsumer<FakeContext>);
        }

        [Fact]
        public void ConfigureApp_WithConfigureConsumerFor_RegistersEventConsumerSettings()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.ConfigureEvents(oe =>
                {
                    oe.ConfigureConsumerFor<FakeContext>();
                });
            });
            // Assert
            var registration = container.GetRegistration<EventConsumerSettings<FakeContext>>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.Lifestyle == Lifestyle.Singleton);
        }

        [Fact]
        public void ConfigureApp_WithConfigureConsumerFor_RegistersSameInstanceOfEventConsumerAsSingletonAndInCollection()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.RegisterTypesFrom(Assembly.GetExecutingAssembly());
                o.RegisterSerializer(SerializationFormat.Json, () => Substitute.For<ITextSerializer>());
                o.ConfigureCommands();
                o.ConfigureQueries();
                o.ConfigureEvents(oe =>
                {
                    oe.ConfigureConsumerFor<FakeContext>();
                });
            });
            // Assert
            var fakeConsumer = container.GetInstance<IEventConsumer<FakeContext>>();
            var consumers = container.GetAllInstances<IEventConsumer>();
            consumers.Should().BeEquivalentTo(new[] { fakeConsumer });
        }

        [Fact]
        public void ConfigureApp_WithConfigureManagerFor_RegistersRecurringCommandManagerAsSingleton()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.ConfigureCommands(oc =>
                {
                    oc.ConfigureManagerFor<FakeContext>();
                });
            });
            // Assert
            var registration = container.GetRegistration<IRecurringCommandManager<FakeContext>>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.ImplementationType == typeof(RecurringCommandManager<FakeContext>)
                                                                            && r.Lifestyle == Lifestyle.Singleton);
        }

        [Fact]
        public void ConfigureApp_WithConfigureManagerFor_RegistersRecurringCommandManagerInCollection()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.RegisterTypesFrom(Assembly.GetExecutingAssembly());
                o.RegisterSerializer(SerializationFormat.Json, () => Substitute.For<ITextSerializer>());
                o.ConfigureQueries();
                o.ConfigureCommands(oc =>
                {
                    oc.RegisterScheduleFactory(RecurringExpressionFormat.Cron, () => Substitute.For<IRecurringScheduleFactory>());
                    oc.ConfigureManagerFor<FakeContext>();
                });
            });
            // Assert
            var registration = container.GetRegistration<IEnumerable<IRecurringCommandManager>>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.Lifestyle == Lifestyle.Singleton);
            var managers = (IEnumerable<IRecurringCommandManager>)registration.GetInstance();
            managers.Should().ContainSingle(c => c is IRecurringCommandManager<FakeContext>);
        }

        [Fact]
        public void ConfigureApp_WithConfigureManagerFor_RegistersRecurringCommandManagerSettings()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.ConfigureCommands(oc =>
                {
                    oc.ConfigureManagerFor<FakeContext>();
                });
            });
            // Assert
            var registration = container.GetRegistration<RecurringCommandManagerSettings<FakeContext>>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.Lifestyle == Lifestyle.Singleton);
        }

        [Fact]
        public void ConfigureApp_WithConfigureManagerFor_RegistersSameinstanceOfRecurringCommandManagerAsSingletonAndInCollection()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.RegisterTypesFrom(Assembly.GetExecutingAssembly());
                o.RegisterSerializer(SerializationFormat.Json, () => Substitute.For<ITextSerializer>());
                o.ConfigureQueries();
                o.ConfigureCommands(oc =>
                {
                    oc.RegisterScheduleFactory(RecurringExpressionFormat.Cron, () => Substitute.For<IRecurringScheduleFactory>());
                    oc.ConfigureManagerFor<FakeContext>();
                });
            });
            // Assert
            var fakeManager = container.GetInstance<IRecurringCommandManager<FakeContext>>();
            var managers = container.GetAllInstances<IRecurringCommandManager>();
            managers.Should().ContainSingle(m => ReferenceEquals(m, fakeManager));
        }

        [Fact]
        public void ConfigureApp_WithRegisterScheduleFactory_RegistersRecurringScheduleFactoryInCollection()
        {
            var fakeScheduleFactory = Substitute.For<IRecurringScheduleFactory>();
            var expectedScheduleFactories = new[] { fakeScheduleFactory };
            // Act
            this.container.ConfigureApp(o =>
            {
                o.ConfigureCommands(oe =>
                {
                    oe.RegisterScheduleFactory(RecurringExpressionFormat.Cron, () => fakeScheduleFactory);
                });
            });
            // Assert
            var registration = container.GetRegistration<IEnumerable<IRecurringScheduleFactory>>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.Lifestyle == Lifestyle.Singleton);
            var scheduleFactories = registration.GetInstance();
            scheduleFactories.Should().BeEquivalentTo(expectedScheduleFactories);
        }

        [Fact]
        public void ConfigureApp_WithRegisterSerializer_RegistersTextSerializerInCollection()
        {
            // Arrange 
            var fakeSerializer = Substitute.For<ITextSerializer>();
            var expectedSerializers = new[] { fakeSerializer };
            // Act
            this.container.ConfigureApp(o =>
            {
                o.RegisterSerializer(SerializationFormat.Json, () => fakeSerializer);
            });
            // Assert
            var registration = container.GetRegistration<IEnumerable<ITextSerializer>>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.Lifestyle == Lifestyle.Singleton);
            var serializers = registration.GetInstance();
            serializers.Should().BeEquivalentTo(expectedSerializers);
        }

        [Fact]
        public void ConfigureApp_WithRegisterTypesFrom_RegistersBoundedContextsAsSingleton()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.RegisterTypesFrom(Assembly.GetExecutingAssembly());
            });
            // Assert
            var registration = container.GetRegistration<FakeContext>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.ImplementationType == typeof(FakeContext)
                                                                            && r.Lifestyle == Lifestyle.Singleton);
        }

        [Fact]
        public void ConfigureApp_WithRegisterTypesFrom_RegistersBoundedContextsInCollection()
        {
            // Arrange 
            var expectedContexts = new BoundedContext[]
            {
                new FakeContext(),
                new DummyContext()
            };
            // Act
            this.container.ConfigureApp(o =>
            {
                o.RegisterTypesFrom(Assembly.GetExecutingAssembly());
            });
            // Assert
            var registration = container.GetRegistration<IEnumerable<BoundedContext>>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.Lifestyle == Lifestyle.Singleton);
            var contexts = registration.GetInstance();
            contexts.Should().BeEquivalentTo(expectedContexts);
        }
        [Fact]
        public void ConfigureApp_WithRegisterTypesFrom_RegistersSameInstancesOfBoundedContextsAsSingletonAndInCollection()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.RegisterTypesFrom(Assembly.GetExecutingAssembly());
            });
            // Assert
            var fakeContext = container.GetInstance<FakeContext>();
            var contexts = container.GetAllInstances<BoundedContext>();
            contexts.Should().ContainSingle(c => ReferenceEquals(c, fakeContext));
        }


        [Fact]
        public void ConfigureApp_WithConfigureCommands_RegistersAsyncCommandHandlers()
        {
            // Arrange
            var expectedChain = new[]
            {
                new RegisteredType(typeof(FakeCommandHandler), Lifestyle.Transient),
                new RegisteredType(typeof(AsyncCommandHandlerWithLogging<FakeCommand>), Lifestyle.Transient),
                new RegisteredType(typeof(AsyncScopedCommandHandler<FakeCommand>), Lifestyle.Singleton)
            };
            // Act
            this.container.ConfigureApp(o =>
            {
                o.RegisterTypesFrom(Assembly.GetExecutingAssembly());
                o.ConfigureCommands();
            });
            // Assert
            var chain = container.GetDecoratorChainOf<IAsyncCommandHandler<FakeCommand>>();
            chain.Should().BeEquivalentTo(expectedChain);
        }

        [Fact]
        public void ConfigureApp_WithConfigureCommands_RegistersCommandProcessor()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.ConfigureCommands();
            });
            // Assert
            var registration = container.GetRegistration<ICommandProcessor>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.ImplementationType == typeof(CommandProcessor)
                                                                            && r.Lifestyle == Lifestyle.Singleton);
        }

        [Fact]
        public void ConfigureApp_WithConfigureCommands_RegistersSyncCommandHandlers()
        {
            // Arrange
            var expectedChain = new[]
            {
                new RegisteredType(typeof(FakeCommandHandler), Lifestyle.Transient),
                new RegisteredType(typeof(SyncCommandHandlerWithLogging<FakeCommand>), Lifestyle.Transient),
                new RegisteredType(typeof(ThreadScopedCommandHandler<FakeCommand>), Lifestyle.Singleton)
            };
            // Act
            this.container.ConfigureApp(o =>
            {
                o.RegisterTypesFrom(Assembly.GetExecutingAssembly());
                o.ConfigureCommands();
            });
            // Assert
            var chain = container.GetDecoratorChainOf<ISyncCommandHandler<FakeCommand>>();
            chain.Should().BeEquivalentTo(expectedChain);
        }

        [Fact]
        public void ConfigureApp_WithConfigureEvents_RegistersAsyncEventHandlers()
        {
            // Arrange
            var expectedChain = new[]
            {
                new RegisteredType(typeof(FakeEventHandler), Lifestyle.Transient),
                new RegisteredType(typeof(AsyncEventHandlerWithLogging<FakeEvent, FakeContext>), Lifestyle.Transient)
            };
            // Act
            this.container.ConfigureApp(o =>
            {
                o.RegisterTypesFrom(Assembly.GetExecutingAssembly());
                o.ConfigureEvents(_ => { });
            });
            // Assert
            var chain = container.GetDecoratorChainOf<IAsyncEventHandler<FakeEvent, FakeContext>>();
            chain.Should().BeEquivalentTo(expectedChain);
        }

        [Fact]
        public void ConfigureApp_WithConfigureEvents_RegistersEventPublisher()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.ConfigureEvents(_ => { });
            });
            // Assert
            var registration = container.GetRegistration<IEventPublisher<FakeContext>>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.ImplementationType == typeof(EventPublisher<FakeContext>)
                                                                            && r.Lifestyle == Lifestyle.Singleton);
        }

        [Fact]
        public void ConfigureApp_WithConfigureEvents_RegistersEventSerializer()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.ConfigureEvents(_ => { });
            });
            // Assert
            var registration = container.GetRegistration<ITextSerializer>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.Lifestyle == Lifestyle.Singleton);
        }

        [Fact]
        public void ConfigureApp_WithConfigureEvents_RegistersSyncEventHandlers()
        {
            // Arrange
            var expectedChain = new[]
            {
                new RegisteredType(typeof(FakeEventHandler), Lifestyle.Transient),
                new RegisteredType(typeof(SyncEventHandlerWithLogging<FakeEvent, FakeContext>), Lifestyle.Transient)
            };
            // Act
            this.container.ConfigureApp(o =>
            {
                o.RegisterTypesFrom(Assembly.GetExecutingAssembly());
                o.ConfigureEvents(_ => { });
            });
            // Assert
            var chain = container.GetDecoratorChainOf<ISyncEventHandler<FakeEvent, FakeContext>>();
            chain.Should().BeEquivalentTo(expectedChain);
        }

        [Fact]
        public void ConfigureApp_WithConfigureLogging_RegistersCustomLoggerFactory()
        {
            // Arrange
            var customLoggerFactory = Substitute.For<ILoggerFactory>();
            // Act
            this.container.ConfigureApp(o =>
            {
                o.ConfigureLogging(() => customLoggerFactory);
            });
            // Assert
            var registration = container.GetRegistration<ILoggerFactory>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.Lifestyle == Lifestyle.Singleton);
            var loggerFactory = registration.GetInstance();
            loggerFactory.Should().Be(customLoggerFactory);
        }

        [Fact]
        public void ConfigureApp_WithConfigureMapping_RegistersMappers()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.RegisterTypesFrom(Assembly.GetExecutingAssembly());
                o.ConfigureMapping();
            });
            // Assert
            var registration = container.GetRegistration<IObjectMapper<FakeEvent, FakeCommand>>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.ImplementationType == typeof(FakeMapper)
                                                                            && r.Lifestyle == Lifestyle.Transient);
        }

        [Fact]
        public void ConfigureApp_WithConfigureMapping_RegistersMappingProcessor()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.ConfigureMapping();
            });
            // Assert
            var registration = container.GetRegistration<IMappingProcessor>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.ImplementationType == typeof(MappingProcessor)
                                                                            && r.Lifestyle == Lifestyle.Singleton);
        }

        [Fact]
        public void ConfigureApp_WithConfigureMapping_RegistersMappingProcessorSettings()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.ConfigureMapping();
            });
            // Assert
            var registration = container.GetRegistration<MappingProcessorSettings>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.Lifestyle == Lifestyle.Singleton);
        }

        [Fact]
        public void ConfigureApp_WithConfigureMapping_RegistersTranslators()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.RegisterTypesFrom(Assembly.GetExecutingAssembly());
                o.ConfigureMapping();
            });
            // Assert
            var registration = container.GetRegistration<IObjectTranslator<FakeEvent, FakeCommand>>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.ImplementationType == typeof(FakeTranslator)
                                                                            && r.Lifestyle == Lifestyle.Transient);
        }
        [Fact]
        public void ConfigureApp_WithConfigureQueries_RegistersAsyncQueryHandlers()
        {
            // Arrange
            var expectedChain = new[]
            {
                new RegisteredType(typeof(FakeQueryHandler), Lifestyle.Transient),
                new RegisteredType(typeof(AsyncQueryHandlerWithLogging<FakeQuery,string>), Lifestyle.Transient),
                new RegisteredType(typeof(AsyncScopedQueryHandler<FakeQuery,string>), Lifestyle.Singleton)
            };
            // Act
            this.container.ConfigureApp(o =>
            {
                o.RegisterTypesFrom(Assembly.GetExecutingAssembly());
                o.ConfigureQueries();
            });
            // Assert
            var chain = container.GetDecoratorChainOf<IAsyncQueryHandler<FakeQuery, string>>();
            chain.Should().BeEquivalentTo(expectedChain);
        }

        [Fact]
        public void ConfigureApp_WithConfigureQueries_RegistersQueryProcessor()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.ConfigureQueries();
            });
            // Assert
            var registration = container.GetRegistration<IQueryProcessor>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.ImplementationType == typeof(QueryProcessor)
                                                                            && r.Lifestyle == Lifestyle.Singleton);
        }

        [Fact]
        public void ConfigureApp_WithConfigureQueries_RegistersSyncQueryHandlers()
        {
            // Arrange
            var expectedChain = new[]
            {
                new RegisteredType(typeof(FakeQueryHandler), Lifestyle.Transient),
                new RegisteredType(typeof(SyncQueryHandlerWithLogging<FakeQuery,string>), Lifestyle.Transient),
                new RegisteredType(typeof(ThreadScopedQueryHandler<FakeQuery,string>), Lifestyle.Singleton)
            };
            // Act
            this.container.ConfigureApp(o =>
            {
                o.RegisterTypesFrom(Assembly.GetExecutingAssembly());
                o.ConfigureQueries();
            });
            // Assert
            var chain = container.GetDecoratorChainOf<ISyncQueryHandler<FakeQuery, string>>();
            chain.Should().BeEquivalentTo(expectedChain);
        }

        #endregion Methods

    }
}