using EnsureThat;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using DDD.Serialization;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Mapping;
    using Application;
    using Domain;
    using DDD;
    using DDD.Core.Infrastructure.Data;

    public static class ContainerExtensions
    {

        #region Methods

        /// <summary>
        /// Gets the decorator chain associated with a specified service.
        /// </summary>
        public static IEnumerable<RegisteredType> GetDecoratorChainOf<TService>(this Container container, bool fromDecorated = true)
            where TService : class
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            var chain = new List<RegisteredType>();
            if (container.TryGetInstance<TService>(out _))
            {
                var serviceTypes = new[]
                {
                    typeof(TService),
                    typeof(Func<TService>)
                };
                var registration = container.GetCurrentRegistrations().FirstOrDefault(r => serviceTypes.Contains(r.ServiceType))?.Registration;
                if (registration != null)
                {
                    if (!serviceTypes.Contains(registration.ImplementationType))
                        chain.Add(new RegisteredType(registration.ImplementationType, registration.Lifestyle));
                    registration = registration.GetRelationships().FirstOrDefault(r => serviceTypes.Contains(r.Dependency.ServiceType))?.Dependency?.Registration;
                    while (registration != null)
                    {
                        if (!serviceTypes.Contains(registration.ImplementationType))
                            chain.Add(new RegisteredType(registration.ImplementationType, registration.Lifestyle));
                        registration = registration.GetRelationships().FirstOrDefault(r => serviceTypes.Contains(r.Dependency.ServiceType))?.Dependency?.Registration;
                    }
                }
                if (fromDecorated)
                    chain.Reverse();
            }
            return chain;
        }

        /// <summary>
        /// Configures application core components.
        /// </summary>
        public static void ConfigureApp(this Container container, Action<AppRegistrationOptions.Builder<Container>> configureOptions)
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            Ensure.That(configureOptions, nameof(configureOptions)).IsNotNull();
            var builder = new AppRegistrationOptions.Builder<Container>();
            var extendableBuilder = (IExtendableRegistrationOptionsBuilder<AppRegistrationOptions, Container>)builder;
            configureOptions(builder);
            var appOptions = extendableBuilder.Build();
            container.Options.DefaultScopedLifestyle = Lifestyle.CreateHybrid(new ThreadScopedLifestyle(), new AsyncScopedLifestyle());
            container.RegisterInstance<IServiceProvider>(container);
            container.RegisterLogger(appOptions);
            container.RegisterBoundedContexts(appOptions);
            container.RegisterSerializers(appOptions);
            foreach (var dbConnectionOptions in appOptions.DbConnectionOptionsCollection)
                container.RegisterDbConnectionProvider(dbConnectionOptions);
            var commandOptions = appOptions.CommandsRegistrationOptions;
            if (commandOptions != null)
            {
                var settings = new CommandProcessorSettings(commandOptions.DefaultValidator);
                container.RegisterInstance(settings);
                container.RegisterSingleton<ICommandProcessor, CommandProcessor>();
                container.RegisterCommandHandlers(appOptions);
                container.RegisterRepositories(appOptions);
                container.RegisterScheduleFactories(commandOptions);
                foreach (var managerOptions in commandOptions.ManagerOptionsCollection)
                    container.RegisterRecurringCommandManager(managerOptions);
            }
            var queryOptions = appOptions.QueriesRegistrationOptions;
            if (queryOptions != null)
            {
                var settings = new QueryProcessorSettings(queryOptions.DefaultValidator);
                container.RegisterInstance(settings);
                container.RegisterSingleton<IQueryProcessor, QueryProcessor>();
                container.RegisterQueryHandlers(appOptions);
            }
            var eventOptions = appOptions.EventsRegistrationOptions;
            if (eventOptions != null)
            {
                container.RegisterSingleton(typeof(IEventPublisher<>), typeof(EventPublisher<>));
                container.RegisterEventHandlers(appOptions);
                container.RegisterEventSerializer(eventOptions);
                foreach (var consumerOptions in eventOptions.ConsumerOptionsCollection)
                    container.RegisterEventConsumer(consumerOptions);
            }
            var mappingOptions = appOptions.MappingRegistrationOptions;
            if (mappingOptions != null)
            {
                var settings = new MappingProcessorSettings(mappingOptions.DefaultMapper, mappingOptions.DefaultTranslator);
                container.RegisterInstance(settings);
                container.RegisterSingleton<IMappingProcessor, MappingProcessor>();
                container.RegisterMappersAndTranslators(appOptions);
            }
            extendableBuilder.ApplyExtensions(container);
        }

        public static void RegisterConditional<TService>(this Container container,
                                                         Func<TService> instanceCreator,
                                                         Lifestyle lifestyle,
                                                         Predicate<PredicateContext> predicate)
                    where TService : class
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            var registration = lifestyle.CreateRegistration(instanceCreator, container);
            container.RegisterConditional<TService>(registration, predicate);
        }

        public static void RegisterConditional<TService>(this Container container, Func<TService> instanceCreator, Predicate<PredicateContext> predicate)
                    where TService : class
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            container.RegisterConditional(instanceCreator, Lifestyle.Transient, predicate);
        }

        public static void RegisterConditional(this Container container,
                                               Type serviceType,
                                               IEnumerable<Assembly> assemblies,
                                               Lifestyle lifestyle,
                                               Func<Type, bool> predicate,
                                               TypesToRegisterOptions options)
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            var implementationTypes = container.GetTypesToRegister(serviceType, assemblies, options)
                                               .Where(predicate);
            var nonGenericTypeDefinitions = implementationTypes.Where(t => t.IsGenericTypeDefinition == false);
            container.Register(serviceType, nonGenericTypeDefinitions, lifestyle);
            var genericTypeDefinitions = implementationTypes.Where(t => t.IsGenericTypeDefinition == true);
            foreach (var genericTypeDefinition in genericTypeDefinitions)
                container.Register(serviceType, genericTypeDefinition, lifestyle);
        }

        public static void RegisterConditional(this Container container,
                                               Type serviceType,
                                               IEnumerable<Assembly> assemblies,
                                               Lifestyle lifestyle,
                                               Func<Type, bool> predicate)
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            container.RegisterConditional(serviceType, assemblies, lifestyle, predicate, new TypesToRegisterOptions());
        }

        public static void RegisterConditional(this Container container,
                                               Type serviceType,
                                               IEnumerable<Assembly> assemblies,
                                               Func<Type, bool> predicate)
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            container.RegisterConditional(serviceType, assemblies, Lifestyle.Transient, predicate);
        }

        public static void RegisterConditional(this Container container,
                                               Type serviceType,
                                               IEnumerable<Assembly> assemblies,
                                               Func<Type, bool> predicate,
                                               TypesToRegisterOptions options)
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            container.RegisterConditional(serviceType, assemblies, Lifestyle.Transient, predicate, options);
        }

        /// <summary>
        /// Tries to get an instance of the specified service.
        /// </summary>
        public static bool TryGetInstance<TService>(this Container container, out TService instance)
             where TService : class
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            IServiceProvider provider = container;
            instance = (TService)provider.GetService(typeof(TService));
            return instance != null;
        }

        private static void RegisterBoundedContexts(this Container container, AppRegistrationOptions options)
        {
            var contextTypes = container.GetTypesToRegister<BoundedContext>(options.AssembliesToScan)
                                        .Where(options.TypeFilter);
            foreach (var contextType in contextTypes)
            {
                container.RegisterSingleton(contextType, contextType);
                container.Collection.Append(typeof(BoundedContext), contextType, Lifestyle.Singleton);
            }
        }

        private static void RegisterCommandHandlers(this Container container, AppRegistrationOptions options)
        {
            var registerOptions = new TypesToRegisterOptions { IncludeGenericTypeDefinitions = true };
            container.RegisterConditional(typeof(ISyncCommandHandler<,>), options.AssembliesToScan, options.TypeFilter, registerOptions);
            container.RegisterDecorator(typeof(ISyncCommandHandler<,>), typeof(SyncCommandHandlerWithLogging<,>));
            container.RegisterDecorator(typeof(ISyncCommandHandler<,>), typeof(ThreadScopedCommandHandler<,>), Lifestyle.Singleton);
            container.RegisterConditional(typeof(IAsyncCommandHandler<,>), options.AssembliesToScan, options.TypeFilter, registerOptions);
            container.RegisterDecorator(typeof(IAsyncCommandHandler<,>), typeof(AsyncCommandHandlerWithLogging<,>));
            container.RegisterDecorator(typeof(IAsyncCommandHandler<,>), typeof(AsyncScopedCommandHandler<,>), Lifestyle.Singleton);
            container.RegisterConditional(typeof(ISyncCommandHandler<>), options.AssembliesToScan, options.TypeFilter);
            container.RegisterDecorator(typeof(ISyncCommandHandler<>), typeof(SyncCommandHandlerWithLogging<>));
            container.RegisterDecorator(typeof(ISyncCommandHandler<>), typeof(ThreadScopedCommandHandler<>), Lifestyle.Singleton);
            container.RegisterConditional(typeof(IAsyncCommandHandler<>), options.AssembliesToScan, options.TypeFilter);
            container.RegisterDecorator(typeof(IAsyncCommandHandler<>), typeof(AsyncCommandHandlerWithLogging<>));
            container.RegisterDecorator(typeof(IAsyncCommandHandler<>), typeof(AsyncScopedCommandHandler<>), Lifestyle.Singleton);
        }

        private static void RegisterEventConsumer(this Container container, EventConsumerOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.ContextType))
                throw new InvalidOperationException($"Context type not specified for an event consumer.");
            var contextType = Type.GetType(options.ContextType);
            var settingsType = typeof(EventConsumerSettings<>).MakeGenericType(contextType);
            var settings = Activator.CreateInstance(settingsType, TimeSpan.FromSeconds(options.ConsumationDelay), options.ConsumationMax);
            var consumerServiceType = typeof(IEventConsumer<>).MakeGenericType(contextType);
            var consumerImplementationType = typeof(EventConsumer<>).MakeGenericType(contextType);
            container.RegisterInstance(settingsType, settings);
            container.RegisterSingleton(consumerServiceType, consumerImplementationType);
            container.Collection.Append(typeof(IEventConsumer), consumerImplementationType, Lifestyle.Singleton);
        }

        private static void RegisterEventHandlers(this Container container, AppRegistrationOptions options)
        {
            container.RegisterConditional(typeof(ISyncEventHandler<,>), options.AssembliesToScan, options.TypeFilter);
            container.RegisterConditional(typeof(IAsyncEventHandler<,>), options.AssembliesToScan, options.TypeFilter);
            container.RegisterDecorator(typeof(ISyncEventHandler<,>), typeof(SyncEventHandlerWithLogging<,>));
            container.RegisterDecorator(typeof(IAsyncEventHandler<,>), typeof(AsyncEventHandlerWithLogging<,>));
        }

        private static void RegisterEventSerializer(this Container container, EventsRegistrationOptions options)
        {
            container.RegisterConditional(() => container.GetAllInstances<ITextSerializer>().First(s => s.Format == options.CurrentSerializationFormat),
                                                Lifestyle.Singleton,
                                                c => c.HasConsumer ? c.Consumer.ImplementationType == typeof(EventTranslator) : true);
        }

        private static void RegisterDbConnectionProvider(this Container container, DbConnectionOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.ContextType))
                throw new InvalidOperationException($"Context type not specified for a database connection provider.");
            var contextType = Type.GetType(options.ContextType);
            var settingsType = typeof(DbConnectionSettings<>).MakeGenericType(contextType);
            var settings = Activator.CreateInstance(settingsType, options.ProviderName, options.ConnectionString);
            var providerServiceType = typeof(IDbConnectionProvider<>).MakeGenericType(contextType);
            var providerImplementationType = typeof(LazyDbConnectionProvider<>).MakeGenericType(contextType);
            container.RegisterInstance(settingsType, settings);
            container.Register(providerServiceType, providerImplementationType, Lifestyle.Scoped);
        }

        private static void RegisterLogger(this Container container, AppRegistrationOptions options)
        {
            container.RegisterSingleton(options.loggerFactoryCreator);
            container.RegisterConditional(typeof(ILogger),
                                          c => typeof(Logger<>).MakeGenericType(c.Consumer.ImplementationType),
                                          Lifestyle.Singleton,
                                          _ => true);
            container.RegisterSingleton(typeof(ILogger<>), typeof(Logger<>));
        }

        private static void RegisterMappersAndTranslators(this Container container, AppRegistrationOptions options)
        {
            container.RegisterConditional(typeof(IObjectMapper<,>), options.AssembliesToScan, options.TypeFilter);
            container.RegisterConditional(typeof(IObjectTranslator<,>), options.AssembliesToScan, options.TypeFilter);
        }

        private static void RegisterQueryHandlers(this Container container, AppRegistrationOptions options)
        {
            var registerOptions = new TypesToRegisterOptions { IncludeGenericTypeDefinitions = true };
            container.RegisterConditional(typeof(ISyncQueryHandler<,,>), options.AssembliesToScan, options.TypeFilter, registerOptions);
            container.RegisterDecorator(typeof(ISyncQueryHandler<,,>), typeof(SyncQueryHandlerWithLogging<,,>));
            container.RegisterDecorator(typeof(ISyncQueryHandler<,,>), typeof(ThreadScopedQueryHandler<,,>), Lifestyle.Singleton);
            container.RegisterConditional(typeof(IAsyncQueryHandler<,,>), options.AssembliesToScan, options.TypeFilter, registerOptions);
            container.RegisterDecorator(typeof(IAsyncQueryHandler<,,>), typeof(AsyncQueryHandlerWithLogging<,,>));
            container.RegisterDecorator(typeof(IAsyncQueryHandler<,,>), typeof(AsyncScopedQueryHandler<,,>), Lifestyle.Singleton);
            container.RegisterConditional(typeof(ISyncQueryHandler<,>), options.AssembliesToScan, options.TypeFilter);
            container.RegisterDecorator(typeof(ISyncQueryHandler<,>), typeof(SyncQueryHandlerWithLogging<,>));
            container.RegisterDecorator(typeof(ISyncQueryHandler<,>), typeof(ThreadScopedQueryHandler<,>), Lifestyle.Singleton);
            container.RegisterConditional(typeof(IAsyncQueryHandler<,>), options.AssembliesToScan, options.TypeFilter);
            container.RegisterDecorator(typeof(IAsyncQueryHandler<,>), typeof(AsyncQueryHandlerWithLogging<,>));
            container.RegisterDecorator(typeof(IAsyncQueryHandler<,>), typeof(AsyncScopedQueryHandler<,>), Lifestyle.Singleton);
        }

        private static void RegisterRecurringCommandManager(this Container container, RecurringCommandManagerOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.ContextType))
                throw new InvalidOperationException($"Context type not specified for a recurring command manager.");
            var contextType = Type.GetType(options.ContextType);
            var settingsType = typeof(RecurringCommandManagerSettings<>).MakeGenericType(contextType);
            var settings = Activator.CreateInstance(settingsType, options.CurrentSerializationFormat, options.CurrentExpressionFormat);
            var managerServiceType = typeof(IRecurringCommandManager<>).MakeGenericType(contextType);
            var managerImplementationType = typeof(RecurringCommandManager<>).MakeGenericType(contextType);
            container.RegisterInstance(settingsType, settings);
            container.RegisterSingleton(managerServiceType, managerImplementationType);
            container.Collection.Append(typeof(IRecurringCommandManager), managerImplementationType, Lifestyle.Singleton);
        }

        private static void RegisterRepositories(this Container container, AppRegistrationOptions options)
        {
            container.RegisterConditional(typeof(ISyncRepository<,>), options.AssembliesToScan, Lifestyle.Scoped, options.TypeFilter);
            container.RegisterConditional(typeof(IAsyncRepository<,>), options.AssembliesToScan, Lifestyle.Scoped, options.TypeFilter);
        }

        private static void RegisterSerializers(this Container container, AppRegistrationOptions options)
        {
            foreach (var serializer in options.Serializers)
                container.Collection.Append(serializer, Lifestyle.Singleton);
        }

        private static void RegisterScheduleFactories(this Container container, CommandsRegistrationOptions options)
        {
            foreach (var scheduleFactory in options.SchedulesFactories)
                container.Collection.Append(scheduleFactory, Lifestyle.Singleton);
        }

        #endregion Methods

    }
}
