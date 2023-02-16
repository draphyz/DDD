﻿using FluentValidation;
using SimpleInjector;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Validation;
    using Application;
    using EnsureThat;

    public static class ContainerExtensions
    {

        #region Methods

        /// <summary>
        /// Registers command and queries validators.
        /// </summary>
        /// <param name="container">The container that registers validators.</param>
        /// <param name="assemblies">The assemblies that contain validators.</param>
        /// <param name="predicate">A predicate that determines which validators should be registered.</param>
        public static void RegisterValidators(this Container container, IEnumerable<Assembly> assemblies, Func<Type, bool> predicate = null)
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            var notNullPredicate = predicate ?? (t => true);
            container.RegisterConditional(typeof(IValidator<>), assemblies, notNullPredicate);
            container.Register(typeof(ISyncCommandValidator<>), typeof(SyncFluentCommandValidatorAdapter<>));
            container.Register(typeof(IAsyncCommandValidator<>), typeof(AsyncFluentCommandValidatorAdapter<>));
            container.Register(typeof(ISyncQueryValidator<>), typeof(SyncFluentQueryValidatorAdapter<>));
            container.Register(typeof(IAsyncQueryValidator<>), typeof(AsyncFluentQueryValidatorAdapter<>));
        }

        #endregion Methods

    }
}
