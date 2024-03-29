﻿using EnsureThat;
using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace DDD.Core.Application
{
    using Domain;
    using Validation;

    /// <summary>
    /// The default command processor for processing and validating commands of any type.  
    /// </summary>
    public class CommandProcessor : ICommandProcessor
    {

        #region Fields

        private readonly IServiceProvider serviceProvider;
        private readonly CommandProcessorSettings settings;
        private static readonly ConcurrentDictionary<BoundedContext, IContextualCommandProcessor> contextualProcessors
            = new ConcurrentDictionary<BoundedContext, IContextualCommandProcessor>();

        #endregion Fields

        #region Constructors

        public CommandProcessor(IServiceProvider serviceProvider, CommandProcessorSettings settings)
        {
            Ensure.That(serviceProvider, nameof(serviceProvider)).IsNotNull();
            Ensure.That(settings, nameof(settings)).IsNotNull();
            this.serviceProvider = serviceProvider;
            this.settings = settings;
        }

        #endregion Constructors

        #region Methods

        public IContextualCommandProcessor<TContext> InGeneric<TContext>(TContext context) where TContext : BoundedContext
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            return (IContextualCommandProcessor<TContext>)contextualProcessors.GetOrAdd(context, _ =>
            new ContextualCommandProcessor<TContext>(this.serviceProvider, context));
        }

        public IContextualCommandProcessor InSpecific(BoundedContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            return contextualProcessors.GetOrAdd(context, _ =>
            {
                var processorType = typeof(ContextualCommandProcessor<>).MakeGenericType(context.GetType());
                return (IContextualCommandProcessor)Activator.CreateInstance(processorType, this.serviceProvider, context);
            });
        }

        public void Process<TCommand>(TCommand command, IMessageContext context) where TCommand : class, ICommand
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            var handler = this.serviceProvider.GetService<ISyncCommandHandler<TCommand>>();
            if (handler == null) throw new InvalidOperationException($"The command handler for type {typeof(ISyncCommandHandler<TCommand>)} could not be found.");
            handler.Handle(command, context);
        }

        public void Process(ICommand command, IMessageContext context)
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            var handlerType = typeof(ISyncCommandHandler<>).MakeGenericType(command.GetType());
            dynamic handler = this.serviceProvider.GetService(handlerType);
            if (handler == null) throw new InvalidOperationException($"The command handler for type {handlerType} could not be found.");
            handler.Handle((dynamic)command, context);
        }

        public Task ProcessAsync<TCommand>(TCommand command, IMessageContext context) where TCommand : class, ICommand
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            var handler = this.serviceProvider.GetService<IAsyncCommandHandler<TCommand>>();
            if (handler == null) throw new InvalidOperationException($"The command handler for type {typeof(IAsyncCommandHandler<TCommand>)} could not be found.");
            return handler.HandleAsync(command, context);
        }

        public Task ProcessAsync(ICommand command, IMessageContext context)
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            var handlerType = typeof(IAsyncCommandHandler<>).MakeGenericType(command.GetType());
            dynamic handler = this.serviceProvider.GetService(handlerType);
            if (handler == null) throw new InvalidOperationException($"The command handler for type {handlerType} could not be found.");
            return handler.HandleAsync((dynamic)command, context);
        }

        public ValidationResult Validate<TCommand>(TCommand command, IValidationContext context) where TCommand : class, ICommand
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            var validator = this.serviceProvider.GetService<ISyncObjectValidator<TCommand>>();
            if (validator == null)
            {
                if (this.settings.DefaultValidator == null)
                    throw new InvalidOperationException($"The command validator for type {typeof(ISyncObjectValidator<TCommand>)} could not be found.");
                return this.settings.DefaultValidator.Validate(command, context);
            }
            return validator.Validate(command, context);
        }

        public ValidationResult Validate(ICommand command, IValidationContext context)
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            var validatorType = typeof(ISyncObjectValidator<>).MakeGenericType(command.GetType());
            dynamic validator = this.serviceProvider.GetService(validatorType);
            if (validator == null)
            {
                if (this.settings.DefaultValidator == null)
                    throw new InvalidOperationException($"The command validator for type {validatorType} could not be found.");
                return this.settings.DefaultValidator.Validate(command, context);
            }
            return validator.Validate((dynamic)command, context);
        }

        public Task<ValidationResult> ValidateAsync<TCommand>(TCommand command, IValidationContext context) where TCommand : class, ICommand
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            var validator = this.serviceProvider.GetService<IAsyncObjectValidator<TCommand>>();
            if (validator == null)
            {
                if (this.settings.DefaultValidator == null)
                    throw new InvalidOperationException($"The command validator for type {typeof(IAsyncObjectValidator<TCommand>)} could not be found.");
                return this.settings.DefaultValidator.ValidateAsync(command, context);
            }
            return validator.ValidateAsync(command, context);
        }

        public Task<ValidationResult> ValidateAsync(ICommand command, IValidationContext context)
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            var validatorType = typeof(IAsyncObjectValidator<>).MakeGenericType(command.GetType());
            dynamic validator = this.serviceProvider.GetService(validatorType);
            if (validator == null)
            {
                if (this.settings.DefaultValidator == null)
                    throw new InvalidOperationException($"The command validator for type {validatorType} could not be found.");
                return this.settings.DefaultValidator.ValidateAsync(command, context);
            }
            return validator.ValidateAsync((dynamic)command, context);
        }

        #endregion Methods
    }
}