using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EnsureThat;
using DDD.DependencyInjection;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Mapping;
    using Core.Application;
    using Core.Domain;

    public static class ContainerExtensions
    {

        #region Methods

        public static TService GetNamedInstance<TService>(this Container container, string name) where TService : class
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            var provider = container.GetInstance<IKeyedServiceProvider<string, TService>>();
            return provider.GetService(name);
        }

        public static void RegisterBoundedContexts(this Container container, IEnumerable<Assembly> assemblies)
        {
            container.Collection.Register<BoundedContext>(assemblies, Lifestyle.Singleton);
        }

        public static void RegisterCommandProcessors(this Container container)
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            container.RegisterSingleton(typeof(IContextualCommandProcessor<>), typeof(ContextualCommandProcessor<>));
            container.RegisterSingleton<ICommandProcessor, CommandProcessor>();
        }

        public static void RegisterCommandHandlers(this Container container, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            var context = new TypesToRegisterOptions { IncludeGenericTypeDefinitions = true };
            container.RegisterConditional(typeof(ISyncCommandHandler<,>), assemblies, predicate, context);
            container.RegisterConditional(typeof(IAsyncCommandHandler<,>), assemblies, predicate, context);
            container.RegisterDecorator(typeof(ISyncCommandHandler<,>), typeof(SyncCommandHandlerWithLogging<,>));
            container.RegisterDecorator(typeof(IAsyncCommandHandler<,>), typeof(AsyncCommandHandlerWithLogging<,>));
            container.RegisterDecorator(typeof(ISyncCommandHandler<,>), typeof(ThreadScopedCommandHandler<,>), Lifestyle.Singleton);
            container.RegisterDecorator(typeof(IAsyncCommandHandler<,>), typeof(AsyncScopedCommandHandler<,>), Lifestyle.Singleton);
            container.RegisterConditional(typeof(ISyncCommandHandler<>), assemblies, predicate);
            container.RegisterConditional(typeof(IAsyncCommandHandler<>), assemblies, predicate);
            container.RegisterDecorator(typeof(ISyncCommandHandler<>), typeof(SyncCommandHandlerWithLogging<>));
            container.RegisterDecorator(typeof(IAsyncCommandHandler<>), typeof(AsyncCommandHandlerWithLogging<>));
            container.RegisterDecorator(typeof(ISyncCommandHandler<>), typeof(ThreadScopedCommandHandler<>), Lifestyle.Singleton);
            container.RegisterDecorator(typeof(IAsyncCommandHandler<>), typeof(AsyncScopedCommandHandler<>), Lifestyle.Singleton);
        }

        public static void RegisterRecurringCommandManager<TContext>(this Container container, RecurringCommandManagerSettings<TContext> settings)
            where TContext : BoundedContext, new()
        {
            container.RegisterInstance(settings);
            container.RegisterSingleton<IRecurringCommandManager<TContext>, RecurringCommandManager<TContext>>();
            container.Collection.Append<IRecurringCommandManager, RecurringCommandManager<TContext>>(Lifestyle.Singleton);
        }

        public static void RegisterQueryProcessors(this Container container)
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            container.RegisterSingleton(typeof(IContextualQueryProcessor<>), typeof(ContextualQueryProcessor<>));
            container.RegisterSingleton<IQueryProcessor, QueryProcessor>();
        }

        public static void RegisterQueryHandlers(this Container container, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            var context = new TypesToRegisterOptions { IncludeGenericTypeDefinitions = true };
            container.RegisterConditional(typeof(ISyncQueryHandler<,,>), assemblies, predicate, context);
            container.RegisterConditional(typeof(IAsyncQueryHandler<,,>), assemblies, predicate, context);
            container.RegisterDecorator(typeof(ISyncQueryHandler<,,>), typeof(SyncQueryHandlerWithLogging<,,>));
            container.RegisterDecorator(typeof(IAsyncQueryHandler<,,>), typeof(AsyncQueryHandlerWithLogging<,,>));
            container.RegisterDecorator(typeof(ISyncQueryHandler<,,>), typeof(ThreadScopedQueryHandler<,,>), Lifestyle.Singleton);
            container.RegisterDecorator(typeof(IAsyncQueryHandler<,,>), typeof(AsyncScopedQueryHandler<,,>), Lifestyle.Singleton);
            container.RegisterConditional(typeof(ISyncQueryHandler<,>), assemblies, predicate);
            container.RegisterConditional(typeof(IAsyncQueryHandler<,>), assemblies, predicate);
            container.RegisterDecorator(typeof(ISyncQueryHandler<,>), typeof(SyncQueryHandlerWithLogging<,>));
            container.RegisterDecorator(typeof(IAsyncQueryHandler<,>), typeof(AsyncQueryHandlerWithLogging<,>));
            container.RegisterDecorator(typeof(ISyncQueryHandler<,>), typeof(ThreadScopedQueryHandler<,>), Lifestyle.Singleton);
            container.RegisterDecorator(typeof(IAsyncQueryHandler<,>), typeof(AsyncScopedQueryHandler<,>), Lifestyle.Singleton);
        }

        public static void RegisterEventPublisher(this Container container)
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            container.RegisterSingleton<IEventPublisher, EventPublisher>();
        }

        public static void RegisterEventHandlers(this Container container, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            var context = new TypesToRegisterOptions { IncludeGenericTypeDefinitions = true, IncludeComposites = false };
            var syncHandlerTypes = container.GetTypesToRegister(typeof(ISyncEventHandler<>), assemblies, context)
                                            .Where(predicate);
            container.Collection.Register(typeof(ISyncEventHandler<>), syncHandlerTypes);
            var asyncHandlerTypes = container.GetTypesToRegister(typeof(IAsyncEventHandler<>), assemblies, context)
                                        .Where(predicate);
            container.Collection.Register(typeof(IAsyncEventHandler<>), asyncHandlerTypes);
            container.RegisterDecorator(typeof(ISyncEventHandler<>), typeof(SyncEventHandlerWithLogging<>));
            container.RegisterDecorator(typeof(IAsyncEventHandler<>), typeof(AsyncEventHandlerWithLogging<>));
        }

        public static void RegisterEventConsumer<TContext>(this Container container, EventConsumerSettings<TContext> settings)
            where TContext : BoundedContext, new()
        {
            container.RegisterInstance(settings);
            container.RegisterSingleton<IEventConsumer<TContext>, EventConsumer<TContext>>();
            container.Collection.Append<IEventConsumer, EventConsumer<TContext>>(Lifestyle.Singleton);
        }

        public static void RegisterMappingProcessor(this Container container)
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            container.RegisterSingleton<IMappingProcessor, MappingProcessor>();
        }

        public static void RegisterMappersAndTranslators(this Container container, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            container.RegisterConditional(typeof(IObjectMapper<,>), assemblies, predicate);
            container.RegisterConditional(typeof(IObjectTranslator<,>), assemblies, predicate);
        }

        public static void RegisterRepositories(this Container container, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            container.RegisterConditional(typeof(ISyncRepository<,>), assemblies, Lifestyle.Scoped, predicate);
            container.RegisterConditional(typeof(IAsyncRepository<,>), assemblies, Lifestyle.Scoped, predicate);
        }

        public static void RegisterConditional<TService>(this Container container, Func<TService> instanceCreator, Lifestyle lifestyle, Predicate<PredicateContext> predicate)
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
                                               Type openGenericServiceType,
                                               IEnumerable<Assembly> assemblies,
                                               Lifestyle lifestyle,
                                               Func<Type, bool> predicate,
                                               TypesToRegisterOptions context)
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            var implementationTypes = container.GetTypesToRegister(openGenericServiceType, assemblies, context)
                                               .Where(predicate);
            foreach (var implementationType in implementationTypes)
                container.Register(openGenericServiceType, implementationType, lifestyle);
        }

        public static void RegisterConditional(this Container container,
                                               Type openGenericServiceType,
                                               IEnumerable<Assembly> assemblies,
                                               Lifestyle lifestyle,
                                               Func<Type, bool> predicate)
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            container.RegisterConditional(openGenericServiceType, assemblies, lifestyle, predicate, new TypesToRegisterOptions());
        }

        public static void RegisterConditional(this Container container,
                                               Type openGenericServiceType,
                                               IEnumerable<Assembly> assemblies,
                                               Func<Type, bool> predicate)
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            container.RegisterConditional(openGenericServiceType, assemblies, Lifestyle.Transient, predicate);
        }

        public static void RegisterConditional(this Container container,
                                               Type openGenericServiceType,
                                               IEnumerable<Assembly> assemblies,
                                               Func<Type, bool> predicate,
                                               TypesToRegisterOptions context)
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            container.RegisterConditional(openGenericServiceType, assemblies, Lifestyle.Transient, predicate, context);
        }

        #endregion Methods
    }
}
