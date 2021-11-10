using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Conditions;
using DDD.DependencyInjection;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Core.Application;

    public static class ContainerExtensions
    {

        #region Methods

        public static TService GetNamedInstance<TService>(this Container container, string name) where TService : class
        {
            Condition.Requires(container, nameof(container)).IsNotNull();
            var provider = container.GetInstance<IKeyedServiceProvider<string, TService>>();
            return provider.GetService(name);
        }

        public static void RegisterConditional<TService>(this Container container, Func<TService> instanceCreator, Predicate<PredicateContext> predicate)
                    where TService : class
        {
            Condition.Requires(container, nameof(container)).IsNotNull();
            var registration = Lifestyle.Transient.CreateRegistration(instanceCreator, container);
            container.RegisterConditional<TService>(registration, predicate);
        }

        public static void RegisterConditional(this Container container, Type openGenericServiceType, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {
            Condition.Requires(container, nameof(container)).IsNotNull();
            var implementationTypes = container.GetTypesToRegister(openGenericServiceType, assemblies)
                                               .Where(predicate);
            container.Register(openGenericServiceType, implementationTypes);
        }

        public static void RegisterEventHandlers(this Container container, params Assembly[] assemblies)
        {
            Condition.Requires(container, nameof(container)).IsNotNull();
            var handlerTypes = container.GetTypesToRegister(typeof(IEventHandler<>), assemblies, new TypesToRegisterOptions
            {
                IncludeGenericTypeDefinitions = true,
                IncludeComposites = false,
            });
            container.Collection.Register(typeof(IEventHandler<>), handlerTypes);
        }

        #endregion Methods

    }
}
