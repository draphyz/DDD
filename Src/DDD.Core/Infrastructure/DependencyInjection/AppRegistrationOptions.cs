using EnsureThat;
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using DDD.Serialization;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Domain;

    public class AppRegistrationOptions
    {

        #region Fields

        private readonly List<Assembly> assembliesToScan;
        private readonly IDictionary<SerializationFormat, Func<ITextSerializer>> serializers;
        private readonly List<DbConnectionOptions> dbConnectionOptionsCollection;

        #endregion Fields

        #region Constructors

        private AppRegistrationOptions() 
        { 
            this.assembliesToScan = new List<Assembly>();
            this.serializers = new Dictionary<SerializationFormat, Func<ITextSerializer>>();
            this.dbConnectionOptionsCollection = new List<DbConnectionOptions>();
            this.loggerFactoryCreator = () => new NullLoggerFactory();
            this.TypeFilter = _ => true;
        }

        #endregion Constructors

        #region Properties

        public IEnumerable<Assembly> AssembliesToScan => this.assembliesToScan;

        public Func<Type, bool> TypeFilter { get; private set; }

        public Func<ILoggerFactory> loggerFactoryCreator { get; private set; }

        public CommandsRegistrationOptions CommandsRegistrationOptions { get; private set; }

        public QueriesRegistrationOptions QueriesRegistrationOptions { get; private set; }

        public EventsRegistrationOptions EventsRegistrationOptions { get; private set; }

        public MappingRegistrationOptions MappingRegistrationOptions { get; private set; }

        public IEnumerable<Func<ITextSerializer>> Serializers => this.serializers.Values;

        public IEnumerable<DbConnectionOptions> DbConnectionOptionsCollection => this.dbConnectionOptionsCollection;

        #endregion Properties

        #region Classes

        public class Builder : IObjectBuilder<AppRegistrationOptions>
        {

            #region Fields

            private readonly AppRegistrationOptions options = new AppRegistrationOptions();

            #endregion Fields

            #region Methods

            public Builder RegisterTypesFrom(IEnumerable<Assembly> assemblies)
            {
                Ensure.That(assemblies, nameof(assemblies)).IsNotNull();
                this.options.assembliesToScan.AddRange(assemblies);
                return this;
            }

            public Builder RegisterTypesFrom(params Assembly[] assemblies)
            {
                Ensure.That(assemblies, nameof(assemblies)).IsNotNull();
                this.options.assembliesToScan.AddRange(assemblies);
                return this;
            }

            public Builder FilterTypesWith(Func<Type, bool> filter)
            {
                Ensure.That(filter, nameof(filter)).IsNotNull();
                this.options.TypeFilter = filter;
                return this;
            }

            public Builder RegisterSerializer(SerializationFormat format, Func<ITextSerializer> serializerCreator)
            {
                Ensure.That(serializerCreator, nameof(serializerCreator)).IsNotNull();
                this.options.serializers[format] = serializerCreator;
                return this;
            }

            public Builder ConfigureDbConnectionFor<TContext>(Action<DbConnectionOptions.Builder<TContext>> configureOptions)
                where TContext : BoundedContext
            {
                Ensure.That(configureOptions, nameof(configureOptions)).IsNotNull();
                var builder = new DbConnectionOptions.Builder<TContext>();
                configureOptions(builder);
                options.dbConnectionOptionsCollection.Add(((IObjectBuilder<DbConnectionOptions>)builder).Build());
                return this;
            }

            public Builder ConfigureDbConnections(IEnumerable<DbConnectionOptions> optionsCollection)
            {
                Ensure.That(optionsCollection, nameof(optionsCollection)).IsNotNull();
                options.dbConnectionOptionsCollection.AddRange(optionsCollection);
                return this;
            }

            public Builder ConfigureDbConnections(params DbConnectionOptions[] optionsCollection)
            {
                Ensure.That(optionsCollection, nameof(optionsCollection)).IsNotNull();
                options.dbConnectionOptionsCollection.AddRange(optionsCollection);
                return this;
            }

            public Builder ConfigureCommands()
            {
                return this.ConfigureCommands(_ => { });
            }

            public Builder ConfigureCommands(Action<CommandsRegistrationOptions.Builder> configureOptions) 
            {
                Ensure.That(configureOptions, nameof(configureOptions)).IsNotNull();
                var builder = new CommandsRegistrationOptions.Builder();
                configureOptions(builder);
                options.CommandsRegistrationOptions = ((IObjectBuilder<CommandsRegistrationOptions>)builder).Build();
                return this;
            }

            public Builder ConfigureQueries()
            {
                return this.ConfigureQueries(_ => { });
            }

            public Builder ConfigureQueries(Action<QueriesRegistrationOptions.Builder> configureOptions)
            {
                Ensure.That(configureOptions, nameof(configureOptions)).IsNotNull();
                var builder = new QueriesRegistrationOptions.Builder();
                configureOptions(builder);
                options.QueriesRegistrationOptions = ((IObjectBuilder<QueriesRegistrationOptions>)builder).Build();
                return this;
            }

            public Builder ConfigureEvents(Action<EventsRegistrationOptions.Builder> configureOptions)
            {
                Ensure.That(configureOptions, nameof(configureOptions)).IsNotNull();
                var builder = new EventsRegistrationOptions.Builder();
                configureOptions(builder);
                options.EventsRegistrationOptions = ((IObjectBuilder<EventsRegistrationOptions>)builder).Build();
                return this;
            }

            public Builder ConfigureMapping()
            {
                return this.ConfigureMapping(_ => { });
            }

            public Builder ConfigureMapping(Action<MappingRegistrationOptions.Builder> configureOptions)
            {
                Ensure.That(configureOptions, nameof(configureOptions)).IsNotNull();
                var builder = new MappingRegistrationOptions.Builder();
                configureOptions(builder);
                options.MappingRegistrationOptions = ((IObjectBuilder<MappingRegistrationOptions>)builder).Build();
                return this;
            }

            public Builder ConfigureLogging(Func<ILoggerFactory> loggerFactoryCreator)
            {
                Ensure.That(loggerFactoryCreator, nameof(loggerFactoryCreator)).IsNotNull();
                this.options.loggerFactoryCreator = loggerFactoryCreator;
                return this;
            }

            AppRegistrationOptions IObjectBuilder<AppRegistrationOptions>.Build() => this.options;

            #endregion Methods

        }

        #endregion Classes

    }
}