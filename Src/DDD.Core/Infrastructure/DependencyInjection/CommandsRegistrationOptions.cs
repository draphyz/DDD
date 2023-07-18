using EnsureThat;
using System;
using System.Collections.Generic;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Application;
    using Validation;
    using Domain;

    public class CommandsRegistrationOptions
    {

        #region Fields

        private readonly List<RecurringCommandManagerOptions> managerOptionsCollection;
        private readonly IDictionary<RecurringExpressionFormat, Func<IRecurringScheduleFactory>> scheduleFactories;

        #endregion Fields

        #region Constructors

        private CommandsRegistrationOptions() 
        { 
            this.managerOptionsCollection = new List<RecurringCommandManagerOptions>();
            this.scheduleFactories = new Dictionary<RecurringExpressionFormat, Func<IRecurringScheduleFactory>>();
        }

        #endregion Constructors

        #region Properties

        public IObjectValidator<ICommand> DefaultValidator { get; private set; }

        public IEnumerable<RecurringCommandManagerOptions> ManagerOptionsCollection => this.managerOptionsCollection;

        public IEnumerable<Func<IRecurringScheduleFactory>> SchedulesFactories => this.scheduleFactories.Values;

        #endregion Properties

        #region Classes

        public class Builder : IObjectBuilder<CommandsRegistrationOptions>
        {

            #region Fields

            private readonly CommandsRegistrationOptions options = new CommandsRegistrationOptions();

            #endregion Fields

            #region Methods

            public Builder ConfigureManagerFor<TContext>()
                where TContext : BoundedContext
            {
                return this.ConfigureManagerFor<TContext>(_ => { });
            }

            public Builder ConfigureManagerFor<TContext>(Action<RecurringCommandManagerOptions.Builder<TContext>> configureOptions)
                where TContext : BoundedContext
            {
                Ensure.That(configureOptions, nameof(configureOptions)).IsNotNull();
                var builder = new RecurringCommandManagerOptions.Builder<TContext>();
                configureOptions(builder);
                options.managerOptionsCollection.Add(((IObjectBuilder<RecurringCommandManagerOptions>)builder).Build());
                return this;
            }

            public Builder ConfigureManagers(IEnumerable<RecurringCommandManagerOptions> optionsCollection)
            {
                Ensure.That(optionsCollection, nameof(optionsCollection)).IsNotNull();
                options.managerOptionsCollection.AddRange(optionsCollection);
                return this;
            }

            public Builder ConfigureManagers(params RecurringCommandManagerOptions[] optionsCollection)
            {
                Ensure.That(optionsCollection, nameof(optionsCollection)).IsNotNull();
                options.managerOptionsCollection.AddRange(optionsCollection);
                return this;
            }

            public Builder RegisterScheduleFactory(RecurringExpressionFormat format, Func<IRecurringScheduleFactory> scheduleFactoryCreator)
            {
                Ensure.That(scheduleFactoryCreator, nameof(scheduleFactoryCreator)).IsNotNull();
                this.options.scheduleFactories[format] = scheduleFactoryCreator;
                return this;
            }

            public Builder RegisterDefaultSuccessfullyValidator()
            {
                Func<ICommand, IValidationContext, ValidationResult> validator = (command, context)
                    => new ValidationResult(true, command.GetType().Name, new ValidationFailure[] { });
                return this.RegisterDefaultValidator(validator);
            }

            public Builder RegisterDefaultValidator(Func<ICommand, IValidationContext, ValidationResult> validator)
            {
                Ensure.That(validator, nameof(validator)).IsNotNull();
                this.options.DefaultValidator = DelegatingValidator<ICommand>.Create(validator);
                return this;
            }
            CommandsRegistrationOptions IObjectBuilder<CommandsRegistrationOptions>.Build() => this.options;

            #endregion Methods

        }

        #endregion Classes

    }
}
