using Conditions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DDD.Core.Application
{
    using DDD.Core.Domain;
    using DependencyInjection;
    using Serialization;
    using Threading;

    public class RecurringCommandManager<TContext> : IRecurringCommandManager<TContext>, IDisposable 
        where TContext : BoundedContext, new()
    {

        #region Fields

        private readonly IQueryProcessor queryProcessor;
        private readonly ICommandProcessor commandProcessor;
        private readonly IKeyedServiceProvider<SerializationFormat, ITextSerializer> commandSerializers;
        private readonly IRecurringScheduleFactory recurringScheduleFactory;
        private readonly ILogger logger;
        private readonly RecurringCommandManagerSettings<TContext> settings;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private Task manageCommands;
        private bool disposed;

        #endregion Fields

        #region Constructors

        public RecurringCommandManager(ICommandProcessor commandProcessor,
                                       IQueryProcessor queryProcessor,
                                       IKeyedServiceProvider<SerializationFormat, ITextSerializer> commandSerializers,
                                       IRecurringScheduleFactory recurringScheduleFactory,
                                       ILogger logger,
                                       RecurringCommandManagerSettings<TContext> settings)
        {
            Condition.Requires(commandProcessor, nameof(commandProcessor)).IsNotNull();
            Condition.Requires(queryProcessor, nameof(queryProcessor)).IsNotNull();
            Condition.Requires(commandSerializers, nameof(commandSerializers)).IsNotNull();
            Condition.Requires(recurringScheduleFactory, nameof(recurringScheduleFactory)).IsNotNull();
            Condition.Requires(logger, nameof(logger)).IsNotNull();
            Condition.Requires(settings, nameof(settings)).IsNotNull();
            this.commandProcessor = commandProcessor;
            this.queryProcessor = queryProcessor;
            this.commandSerializers = commandSerializers;
            this.recurringScheduleFactory = recurringScheduleFactory;
            this.logger = logger;
            this.settings = settings;
        }

        #endregion Constructors

        #region Properties

        public TContext Context => this.settings.Context;

        public bool IsRunning { get; private set; }

        protected CancellationToken CancellationToken => this.cancellationTokenSource.Token;

        #endregion Properties

        #region Methods

        public async Task RegisterAsync(ICommand command, string recurringExpression, CancellationToken cancellationToken = default)
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            Condition.Requires(recurringExpression, nameof(recurringExpression)).IsNotNull();
            this.ValidateRecurringExpression(recurringExpression);
            await new SynchronizationContextRemover();
            var commandSerializer = this.commandSerializers.GetService(this.settings.CurrentSerializationFormat);
            var commandId = this.queryProcessor.In<TContext>().Process(new GenerateRecurringCommandId(), MessageContext.CancellableContext(cancellationToken));
            var registrationCommand = new RegisterRecurringCommand
            {
                CommandId = commandId,
                CommandType = command.GetType().ShortAssemblyQualifiedName(),
                Body = commandSerializer.SerializeToString(command),
                BodyFormat = commandSerializer.Format.ToString().ToUpper(),
                RecurringExpression = recurringExpression
            };
            await this.commandProcessor.In<TContext>().ProcessAsync(registrationCommand, MessageContext.CancellableContext(cancellationToken));
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
                this.logger.LogInformation("Recurring command management in the context '{Context}' has started.", this.Context.Name);
                this.logger.LogDebug("The configuration settings of recurring command management in the context '{Context}' are the following : {@Settings}", this.Context.Name, this.settings);
                this.CancellationToken.ThrowIfCancellationRequested(); 
                await new SynchronizationContextRemover();
                var commandInfos = await this.FindAllRecurringCommandsAsync();
                var tasks = new List<Task>();
                foreach (var commandInfo in commandInfos)
                    tasks.Add(ManageCommandAsync(commandInfo.RecurringCommand, commandInfo.RecurringSchedule));
                await Task.WhenAll(tasks);
                if (this.CancellationToken.IsCancellationRequested)
                    this.logger.LogInformation("Recurring command management in the context '{Context}' has been stopped.", this.Context.Name);
                else
                    this.logger.LogInformation("Recurring command management in the context '{Context}' has finished.", this.Context.Name);
            }
            catch (OperationCanceledException)
            {
                this.logger.LogInformation("Recurring command management in the context '{Context}' has been stopped.", this.Context.Name);
            }
            catch (Exception exception)
            {
                this.logger.LogError(default, exception, "An error occurred during recurring command management in the context '{Context}'.", this.Context.Name);
            }
            finally
            {
                this.IsRunning = false;
            }
        }

        private async Task<IEnumerable<(RecurringCommand RecurringCommand, IRecurringSchedule RecurringSchedule)>> FindAllRecurringCommandsAsync()
        {
            var recurringCommands = await this.queryProcessor.In<TContext>().ProcessAsync(new FindRecurringCommands(), MessageContext.CancellableContext(this.CancellationToken));
            var results = new List<(RecurringCommand, IRecurringSchedule)>();
            foreach (var recurringCommand in recurringCommands)
                results.Add((recurringCommand, this.recurringScheduleFactory.Create(recurringCommand.RecurringExpression)));
            return results;
        }

        private async Task ManageCommandAsync(RecurringCommand recurringCommand, IRecurringSchedule recurringSchedule)
        {
            try
            {
                this.logger.LogInformation("The management of the recurring command {CommandId} in the context '{Context}' has started.", recurringCommand.CommandId, this.Context.Name);
                var command = this.DeserializeCommand(recurringCommand);
                var now = SystemTime.Local();
                var nextOccurrence = recurringSchedule.GetNextOccurrence(now);
                while (nextOccurrence.HasValue)
                {
                    this.CancellationToken.ThrowIfCancellationRequested();
                    this.logger.LogDebug("The next processing of the recurring command {CommandId} in the context '{Context}' has been scheduled for {CommandExecutionTime}.", recurringCommand.CommandId, this.Context.Name, nextOccurrence);
                    bool success;
                    try
                    {
                        await this.commandProcessor.ProcessWithDelayAsync(command, nextOccurrence.Value - now, MessageContext.CancellableContext(this.CancellationToken));
                        success = true;
                    }
                    catch (Exception exception) when (!(exception is OperationCanceledException))
                    {
                        success = false;
                        this.logger.LogError(default, exception, "An error occurred during the processing of the recurring command {CommandId} in the context '{Context}'.", recurringCommand.CommandId, this.Context.Name);
                        var failureCommand = new MarkRecurringCommandAsFailed
                        {
                            CommandId = recurringCommand.CommandId,
                            ExecutionTime = nextOccurrence.Value,
                            ExceptionInfo = exception.ToString()
                        };
                        await this.commandProcessor.In<TContext>().ProcessAsync(failureCommand);
                    }
                    if (success)
                    {
                        this.logger.LogDebug("The processing of the recurring command {CommandId} in the context '{Context}' has successfully finished.", recurringCommand.CommandId, this.Context.Name);
                        var successCommand = new MarkRecurringCommandAsSuccessful
                        {
                            CommandId = recurringCommand.CommandId,
                            ExecutionTime = nextOccurrence.Value
                        };
                        await this.commandProcessor.In<TContext>().ProcessAsync(successCommand);
                    }
                    now = SystemTime.Local();
                    nextOccurrence = recurringSchedule.GetNextOccurrence(now);
                }
                if (this.CancellationToken.IsCancellationRequested)
                    this.logger.LogInformation("The management of the recurring command {CommandId} in the context '{Context}' has been stopped.", recurringCommand.CommandId, this.Context.Name);
                else
                    this.logger.LogInformation("The management of the recurring command {CommandId} in the context '{Context}' has finished.", recurringCommand.CommandId, this.Context.Name);
            }
            catch(OperationCanceledException)
            {
                this.logger.LogInformation("The management of the recurring command {CommandId} in the context '{Context}' has been stopped.", recurringCommand.CommandId, this.Context.Name);
                throw;
            }
            catch (Exception exception) 
            {
                this.logger.LogError(default, exception, "An error occurred during the management of the recurring command {CommandId} in the context '{Context}'.", recurringCommand.CommandId, this.Context.Name);
            }
        }

        private ICommand DeserializeCommand(RecurringCommand recurringCommand)
        {
            var format = (SerializationFormat)Enum.Parse(typeof(SerializationFormat), recurringCommand.BodyFormat, ignoreCase: true);
            var type = Type.GetType(recurringCommand.CommandType);
            var serializer = this.commandSerializers.GetService(format);
            return (ICommand)serializer.DeserializeFromString(recurringCommand.Body, type);
        }

        private void ValidateRecurringExpression(string recurringExpression)
        {
            try
            {
                this.recurringScheduleFactory.Create(recurringExpression);
            }
            catch (Exception ex) 
            {
                throw new ArgumentException(
                    "The recurring expression is invalid. Please see the inner exception for details.",
                    nameof(recurringExpression),
                    ex);
            }
        }

        #endregion Methods

    }
}
