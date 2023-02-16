using EnsureThat;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using DDD.DependencyInjection;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Mapping;
    using Application;
    using Domain;

    public static class ContainerExtensions
    {

        #region Methods
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
        /// Gets a named instance of a service.
        /// </summary>
        public static TService GetNamedInstance<TService>(this Container container, string name) where TService : class
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            var provider = container.GetInstance<IKeyedServiceProvider<string, TService>>();
            return provider.GetService(name);
        }

        /// <summary>
        /// Registers base components for handling commands, queries and events.
        /// </summary>
        /// <param name="container">The container that registers base components.</param>
        /// <param name="assemblies">The assemblies that contain base components.</param>
        /// <param name="predicate">A predicate that determines which components should be registered.</param>
        public static void RegisterBaseComponents(this Container container, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate = null)
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            Ensure.That(assemblies, nameof(assemblies)).IsNotNull(); ;
            var notNullPredicate = predicate ?? (t => true);
            container.Options.DefaultScopedLifestyle = Lifestyle.CreateHybrid(new ThreadScopedLifestyle(), new AsyncScopedLifestyle());
            container.RegisterInstance<IServiceProvider>(container);
            container.Collection.Register<BoundedContext>(assemblies, Lifestyle.Singleton);
            container.RegisterSingleton<ICommandProcessor, CommandProcessor>();
            container.RegisterCommandHandlers(assemblies, notNullPredicate);
            container.RegisterSingleton<IQueryProcessor, QueryProcessor>();
            container.RegisterQueryHandlers(assemblies, notNullPredicate);
            container.RegisterSingleton(typeof(IEventPublisher<>), typeof(EventPublisher<>));
            container.RegisterEventHandlers(assemblies, notNullPredicate);
            container.RegisterSingleton<IMappingProcessor, MappingProcessor>();
            container.RegisterMappersAndTranslators(assemblies, notNullPredicate);
            container.RegisterRepositories(assemblies, notNullPredicate);
        }

        /// <summary>
        /// Registers an event consumer for a specific bounded context.
        /// </summary>
        public static void RegisterEventConsumer<TContext>(this Container container, EventConsumerSettings<TContext> settings)
            where TContext : BoundedContext, new()
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            Ensure.That(settings, nameof(settings)).IsNotNull();
            container.RegisterInstance(settings);
            container.RegisterSingleton<IEventConsumer<TContext>, EventConsumer<TContext>>();
            container.Collection.Append<IEventConsumer, EventConsumer<TContext>>(Lifestyle.Singleton);
        }

        /// <summary>
        /// Registers a recurring command manager for a specific bounded context.
        /// </summary>
        public static void RegisterRecurringCommandManager<TContext>(this Container container, RecurringCommandManagerSettings<TContext> settings)
            where TContext : BoundedContext, new()
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            Ensure.That(settings, nameof(settings)).IsNotNull();
            container.RegisterInstance(settings);
            container.RegisterSingleton<IRecurringCommandManager<TContext>, RecurringCommandManager<TContext>>();
            container.Collection.Append<IRecurringCommandManager, RecurringCommandManager<TContext>>(Lifestyle.Singleton);
        }

        /// <summary>
        /// Registers loggers.
        /// </summary>
        public static void RegisterLoggers(this Container container, ILoggerFactory loggerFactory)
        {
            container.RegisterInstance(loggerFactory);
            container.RegisterConditional(typeof(ILogger),
                                          c => typeof(Logger<>).MakeGenericType(c.Consumer.ImplementationType),
                                          Lifestyle.Singleton,
                                          _ => true);
            container.RegisterSingleton(typeof(ILogger<>), typeof(Logger<>));
        }

        private static void RegisterCommandHandlers(this Container container, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {
            var options = new TypesToRegisterOptions { IncludeGenericTypeDefinitions = true };
            container.RegisterConditional(typeof(ISyncCommandHandler<,>), assemblies, predicate, options);
            container.RegisterDecorator(typeof(ISyncCommandHandler<,>), typeof(SyncCommandHandlerWithLogging<,>));
            container.RegisterDecorator(typeof(ISyncCommandHandler<,>), typeof(ThreadScopedCommandHandler<,>), Lifestyle.Singleton);
            container.RegisterConditional(typeof(IAsyncCommandHandler<,>), assemblies, predicate, options);
            container.RegisterDecorator(typeof(IAsyncCommandHandler<,>), typeof(AsyncCommandHandlerWithLogging<,>));
            container.RegisterDecorator(typeof(IAsyncCommandHandler<,>), typeof(AsyncScopedCommandHandler<,>), Lifestyle.Singleton);
            container.RegisterConditional(typeof(ISyncCommandHandler<>), assemblies, predicate);
            container.RegisterDecorator(typeof(ISyncCommandHandler<>), typeof(SyncCommandHandlerWithLogging<>));
            container.RegisterDecorator(typeof(ISyncCommandHandler<>), typeof(ThreadScopedCommandHandler<>), Lifestyle.Singleton);
            container.RegisterConditional(typeof(IAsyncCommandHandler<>), assemblies, predicate);
            container.RegisterDecorator(typeof(IAsyncCommandHandler<>), typeof(AsyncCommandHandlerWithLogging<>));
            container.RegisterDecorator(typeof(IAsyncCommandHandler<>), typeof(AsyncScopedCommandHandler<>), Lifestyle.Singleton);
        }

        private static void RegisterQueryHandlers(this Container container, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {
            var options = new TypesToRegisterOptions { IncludeGenericTypeDefinitions = true };
            container.RegisterConditional(typeof(ISyncQueryHandler<,,>), assemblies, predicate, options);
            container.RegisterDecorator(typeof(ISyncQueryHandler<,,>), typeof(SyncQueryHandlerWithLogging<,,>));
            container.RegisterDecorator(typeof(ISyncQueryHandler<,,>), typeof(ThreadScopedQueryHandler<,,>), Lifestyle.Singleton);
            container.RegisterConditional(typeof(IAsyncQueryHandler<,,>), assemblies, predicate, options);
            container.RegisterDecorator(typeof(IAsyncQueryHandler<,,>), typeof(AsyncQueryHandlerWithLogging<,,>));
            container.RegisterDecorator(typeof(IAsyncQueryHandler<,,>), typeof(AsyncScopedQueryHandler<,,>), Lifestyle.Singleton);
            container.RegisterConditional(typeof(ISyncQueryHandler<,>), assemblies, predicate);
            container.RegisterDecorator(typeof(ISyncQueryHandler<,>), typeof(SyncQueryHandlerWithLogging<,>));
            container.RegisterDecorator(typeof(ISyncQueryHandler<,>), typeof(ThreadScopedQueryHandler<,>), Lifestyle.Singleton);
            container.RegisterConditional(typeof(IAsyncQueryHandler<,>), assemblies, predicate);
            container.RegisterDecorator(typeof(IAsyncQueryHandler<,>), typeof(AsyncQueryHandlerWithLogging<,>));
            container.RegisterDecorator(typeof(IAsyncQueryHandler<,>), typeof(AsyncScopedQueryHandler<,>), Lifestyle.Singleton);
        }

        private static void RegisterEventHandlers(this Container container, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {
            container.RegisterConditional(typeof(ISyncEventHandler<,>), assemblies, predicate);
            container.RegisterConditional(typeof(IAsyncEventHandler<,>), assemblies, predicate);
            container.RegisterDecorator(typeof(ISyncEventHandler<,>), typeof(SyncEventHandlerWithLogging<,>));
            container.RegisterDecorator(typeof(IAsyncEventHandler<,>), typeof(AsyncEventHandlerWithLogging<,>));
        }

        private static void RegisterMappersAndTranslators(this Container container, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {
            container.RegisterConditional(typeof(IObjectMapper<,>), assemblies, predicate);
            container.RegisterConditional(typeof(IObjectTranslator<,>), assemblies, predicate);
        }

        private static void RegisterRepositories(this Container container, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {
            container.RegisterConditional(typeof(ISyncRepository<,>), assemblies, Lifestyle.Scoped, predicate);
            container.RegisterConditional(typeof(IAsyncRepository<,>), assemblies, Lifestyle.Scoped, predicate);
        }

        #endregion Methods
    }
}
