using Conditions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DDD.Core.Application
{
    using DependencyInjection;
    using Serialization;
    using Threading;

    public class RecurringCommandManager<TContext> : IRecurringCommandManager, IDisposable 
        where TContext : class, IBoundedContext
    {

        #region Fields

        private readonly IQueryProcessor<TContext> coreQueryProcessor;
        private readonly ICommandProcessor<TContext> coreCommandProcessor;
        private readonly ICommandProcessor recurringCommandProcessor;
        private readonly IKeyedServiceProvider<SerializationFormat, ITextSerializer> commandSerializers;
        private readonly ICronScheduleFactory cronScheduleFactory;
        private readonly ILogger logger;
        private readonly RecurringCommandManagerSettings settings;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private Task manageCommands;
        private bool disposed;

        #endregion Fields

        #region Constructors

        public RecurringCommandManager(ICommandProcessor<TContext> coreCommandProcessor,
                                       IQueryProcessor<TContext> coreQueryProcessor,
                                       ICommandProcessor recurringCommandProcessor,
                                       IKeyedServiceProvider<SerializationFormat, ITextSerializer> commandSerializers,
                                       ICronScheduleFactory cronScheduleFactory,
                                       ILogger logger,
                                       RecurringCommandManagerSettings settings)
        {
            Condition.Requires(coreCommandProcessor, nameof(coreCommandProcessor)).IsNotNull();
            Condition.Requires(coreQueryProcessor, nameof(coreQueryProcessor)).IsNotNull();
            Condition.Requires(recurringCommandProcessor, nameof(recurringCommandProcessor)).IsNotNull();
            Condition.Requires(commandSerializers, nameof(commandSerializers)).IsNotNull();
            Condition.Requires(cronScheduleFactory, nameof(cronScheduleFactory)).IsNotNull();
            Condition.Requires(logger, nameof(logger)).IsNotNull();
            Condition.Requires(settings, nameof(settings)).IsNotNull();
            this.coreCommandProcessor = coreCommandProcessor;
            this.coreQueryProcessor = coreQueryProcessor;
            this.recurringCommandProcessor = recurringCommandProcessor;
            this.commandSerializers = commandSerializers;
            this.cronScheduleFactory = cronScheduleFactory;
            this.logger = logger;
            this.settings = settings;
        }

        #endregion Constructors

        #region Properties

        public TContext Context => this.coreCommandProcessor.Context;

        public bool IsRunning { get; private set; }

        protected CancellationToken CancellationToken => this.cancellationTokenSource.Token;

        #endregion Properties

        #region Methods

        public async Task RegisterAsync(ICommand command, string cronExpression, CancellationToken cancellationToken = default)
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            Condition.Requires(cronExpression, nameof(cronExpression)).IsNotNull();
            this.ValidateCronExpression(cronExpression);
            await new SynchronizationContextRemover();
            var commandSerializer = this.commandSerializers.GetService(this.settings.CurrentSerializationFormat);
            var commandId = this.coreQueryProcessor.Process(new GenerateRecurringCommandId(), MessageContext.CancellableContext(cancellationToken));
            var registerCommand = new RegisterRecurringCommand
            {
                CommandId = commandId,
                CommandType = command.GetType().ShortAssemblyQualifiedName(),
                Body = commandSerializer.SerializeToString(command),
                BodyFormat = commandSerializer.Format.ToString().ToUpper(),
                CronExpression = cronExpression
            };
            await this.coreCommandProcessor.ProcessAsync(registerCommand, MessageContext.CancellableContext(cancellationToken));
        }

        public void Start()
        {
            if (!this.IsRunning)
            {
                this.IsRunning = true;
                this.manageCommands = Task.Run(async () => await ManageCommandsAsync());
            }
        }

        public void Stop()
        {
            if (this.IsRunning)
            {
                this.cancellationTokenSource.Cancel();
                this.manageCommands.Wait();
            }    
        }

        public void Wait(TimeSpan? timeout = null)
        {
            if (this.IsRunning)
            {
                if (timeout.HasValue)
                    this.manageCommands.Wait(timeout.Value);
                else
                    this.manageCommands.Wait();
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    this.Stop();
                    this.cancellationTokenSource.Dispose();
                }
                disposed = true;
            }
        }

        private async Task ManageCommandsAsync()
        {
            try
            {
                this.logger.LogInformation("La gestion des commandes récurrentes dans le contexte '{Context}' a commencé.", this.Context.Name);
                this.logger.LogDebug("Les paramètres de configuration de gestion des commandes récurrentes dans le contexte '{Context}' sont les suivants : {@Settings}", this.Context.Name, this.settings);
                this.CancellationToken.ThrowIfCancellationRequested(); 
                await new SynchronizationContextRemover();
                var commandInfos = await this.FindAllRecurringCommandsAsync();
                var tasks = new List<Task>();
                foreach (var commandInfo in commandInfos)
                    tasks.Add(ManageCommandAsync(commandInfo.RecurringCommand, commandInfo.CronSchedule));
                await Task.WhenAll(tasks);
                if (this.CancellationToken.IsCancellationRequested)
                    this.logger.LogInformation("La gestion des commandes récurrentes dans le contexte '{Context}' a été interrompue.", this.Context.Name);
                else
                    this.logger.LogInformation("La gestion des commandes récurrentes dans le contexte '{Context}' s'est terminée.", this.Context.Name);
            }
            catch (OperationCanceledException)
            {
                this.logger.LogInformation("La gestion des commandes récurrentes dans le contexte '{Context}' a été interrompue.", this.Context.Name);
            }
            catch (Exception exception)
            {
                this.logger.LogError(default, exception, "Une erreur s'est produite durant le traitement des commandes récurrentes dans le contexte '{Context}'.", this.Context.Name);
            }
            finally
            {
                this.IsRunning = false;
            }
        }

        private async Task<IEnumerable<(RecurringCommand RecurringCommand, ICronSchedule CronSchedule)>> FindAllRecurringCommandsAsync()
        {
            var recurringCommands = await this.coreQueryProcessor.ProcessAsync(new FindRecurringCommands(), MessageContext.CancellableContext(this.CancellationToken));
            var results = new List<(RecurringCommand, ICronSchedule)>();
            foreach (var recurringCommand in recurringCommands)
                results.Add((recurringCommand, this.cronScheduleFactory.Create(recurringCommand.CronExpression)));
            return results;
        }

        private async Task ManageCommandAsync(RecurringCommand recurringCommand, ICronSchedule cronSchedule)
        {
            try
            {
                this.logger.LogInformation("La gestion de la commande récurrente {CommandId} dans le contexte '{Context}' a commencé.", recurringCommand.CommandId, this.Context.Name);
                var command = this.DeserializeCommand(recurringCommand);
                var now = SystemTime.Local();
                var nextOccurrence = cronSchedule.GetNextOccurence(now);
                while (nextOccurrence.HasValue)
                {
                    this.CancellationToken.ThrowIfCancellationRequested();
                    this.logger.LogDebug("Le prochain traitement de la commande récurrente {CommandId} dans le contexte '{Context}' a été planifié au {CommandExecutionTime}.", recurringCommand.CommandId, this.Context.Name, nextOccurrence);
                    bool success;
                    try
                    {
                        await this.recurringCommandProcessor.ProcessWithDelayAsync(command, nextOccurrence.Value - now, MessageContext.CancellableContext(this.CancellationToken));
                        success = true;
                    }
                    catch (Exception exception) when (!(exception is OperationCanceledException))
                    {
                        success = false;
                        this.logger.LogError(default, exception, "Une erreur s'est produite durant le traitement de la commande récurrente {CommandId} dans le contexte '{Context}'.", recurringCommand.CommandId, this.Context.Name);
                        await this.coreCommandProcessor.ProcessAsync(new MarkRecurringCommandAsFailed { CommandId = recurringCommand.CommandId, ExecutionTime = nextOccurrence.Value, ExceptionInfo = exception.ToString() });
                    }
                    if (success)
                    {
                        this.logger.LogDebug("Le traitement de la commande récurrente {CommandId} dans le contexte '{Context}' s'est terminé avec succès.", recurringCommand.CommandId, this.Context.Name);
                        await this.coreCommandProcessor.ProcessAsync(new MarkRecurringCommandAsSuccessful { CommandId = recurringCommand.CommandId, ExecutionTime = nextOccurrence.Value });
                    }
                    now = SystemTime.Local();
                    nextOccurrence = cronSchedule.GetNextOccurence(now);
                }
                if (this.CancellationToken.IsCancellationRequested)
                    this.logger.LogInformation("La gestion de la commande récurrente {CommandId} dans le contexte '{Context}' a été interrompue.", recurringCommand.CommandId, this.Context.Name);
                else
                    this.logger.LogInformation("La gestion de la commande récurrente {CommandId} dans le contexte '{Context}' s'est terminée.", recurringCommand.CommandId, this.Context.Name);
            }
            catch(OperationCanceledException)
            {
                this.logger.LogInformation("La gestion de la commande récurrente {CommandId} dans le contexte '{Context}' a été interrompue.", recurringCommand.CommandId, this.Context.Name);
                throw;
            }
            catch (Exception exception) 
            {
                this.logger.LogError(default, exception, "Une erreur s'est produite durant la gestion de la commande récurrente {CommandId} dans le contexte '{Context}'.", recurringCommand.CommandId, this.Context.Name);
            }
        }

        private ICommand DeserializeCommand(RecurringCommand recurringCommand)
        {
            var format = (SerializationFormat)Enum.Parse(typeof(SerializationFormat), recurringCommand.BodyFormat, ignoreCase: true);
            var type = Type.GetType(recurringCommand.CommandType);
            var serializer = this.commandSerializers.GetService(format);
            return (ICommand)serializer.DeserializeFromString(recurringCommand.Body, type);
        }

        private void ValidateCronExpression(string cronExpression)
        {
            try
            {
                this.cronScheduleFactory.Create(cronExpression);
            }
            catch (Exception ex) 
            {
                throw new ArgumentException(
                    "The cron expression is invalid. Please see the inner exception for details.",
                    nameof(cronExpression),
                    ex);
            }
        }

        #endregion Methods

    }
}
