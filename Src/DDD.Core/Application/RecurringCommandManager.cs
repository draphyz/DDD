using EnsureThat;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DDD.Core.Application
{
    using Domain;
    using Serialization;
    using Threading;
    using DDD;

    public class RecurringCommandManager<TContext> : IRecurringCommandManager<TContext>, IDisposable 
        where TContext : BoundedContext
    {

        #region Fields

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ICommandProcessor commandProcessor;
        private readonly IEnumerable<ITextSerializer> commandSerializers;
        private readonly ILogger logger;
        private readonly IQueryProcessor queryProcessor;
        private readonly IEnumerable<IRecurringScheduleFactory> recurringScheduleFactories;
        private readonly RecurringCommandManagerSettings<TContext> settings;
        private bool disposed;
        private Task manageCommands;

        #endregion Fields

        #region Constructors

        public RecurringCommandManager(TContext context,
                                       ICommandProcessor commandProcessor,
                                       IQueryProcessor queryProcessor,
                                       IEnumerable<ITextSerializer> commandSerializers,
                                       IEnumerable<IRecurringScheduleFactory> recurringScheduleFactories,
                                       ILogger logger,
                                       RecurringCommandManagerSettings<TContext> settings)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            Ensure.That(commandProcessor, nameof(commandProcessor)).IsNotNull();
            Ensure.That(queryProcessor, nameof(queryProcessor)).IsNotNull();
            Ensure.That(commandSerializers, nameof(commandSerializers)).IsNotNull();
            Ensure.That(recurringScheduleFactories, nameof(recurringScheduleFactories)).IsNotNull();
            Ensure.That(logger, nameof(logger)).IsNotNull();
            Ensure.That(settings, nameof(settings)).IsNotNull();
            this.Context = context;
            this.commandProcessor = commandProcessor;
            this.queryProcessor = queryProcessor;
            this.commandSerializers = commandSerializers;
            this.recurringScheduleFactories = recurringScheduleFactories;
            this.logger = logger;
            this.settings = settings;
        }

        #endregion Constructors

        #region Properties

        public TContext Context { get; }

        public bool IsRunning { get; private set; }

        protected CancellationToken CancellationToken => this.cancellationTokenSource.Token;

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async Task RegisterAsync(ICommand command, string recurringExpression, CancellationToken cancellationToken = default)
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            Ensure.That(recurringExpression, nameof(recurringExpression)).IsNotNull();
            var recurringScheduleFactory = this.GetScheduleFactory(this.settings.CurrentExpressionFormat);
            ValidateRecurringExpression(recurringScheduleFactory, recurringExpression);
            await new SynchronizationContextRemover();
            var serializer = this.GetSerializer(this.settings.CurrentSerializationFormat);
            var commandId = this.queryProcessor.InGeneric(Context).Process(new GenerateRecurringCommandId(), MessageContext.CancellableContext(cancellationToken));
            var registrationCommand = new RegisterRecurringCommand
            {
                CommandId = commandId,
                CommandType = command.GetType().ShortAssemblyQualifiedName(),
                Body = serializer.SerializeToString(command),
                BodyFormat = serializer.Format.ToString().ToUpper(),
                RecurringExpression = recurringExpression,
                RecurringExpressionFormat = recurringScheduleFactory.Format.ToString().ToUpper(),
            };
            await this.commandProcessor.InGeneric(Context).ProcessAsync(registrationCommand, MessageContext.CancellableContext(cancellationToken));
        }

        public void Start()
        {
            if (!this.IsRunning)
            {
                this.logger.LogInformation("RecurringCommandManager for the context '{Context}' is starting.", this.Context.Name);
                this.logger.LogDebug("The manager settings for the context '{Context}' are as follows : {Settings}", this.Context.Name, this.settings);
                this.IsRunning = true;
                this.manageCommands = Task.Run(async () => await ManageCommandsAsync());
                this.logger.LogInformation("RecurringCommandManager for the context '{Context}' has started.", this.Context.Name);
            }
        }

        public void Stop()
        {
            if (this.IsRunning)
            {
                this.logger.LogInformation("RecurringCommandManager for the context '{Context}' is stopping.", this.Context.Name);
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

        private static void ValidateRecurringExpression(IRecurringScheduleFactory recurringScheduleFactory, string recurringExpression)
        {
            try
            {
                recurringScheduleFactory.Create(recurringExpression);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    "The recurring expression is invalid. Please see the inner exception for details.",
                    nameof(recurringExpression),
                    ex);
            }
        }

        private ICommand DeserializeCommand(RecurringCommand recurringCommand)
        {
            var format = (SerializationFormat)Enum.Parse(typeof(SerializationFormat), recurringCommand.BodyFormat, ignoreCase: true);
            var type = Type.GetType(recurringCommand.CommandType);
            var serializer = this.GetSerializer(format);
            return (ICommand)serializer.DeserializeFromString(recurringCommand.Body, type);
        }

        private async Task<IEnumerable<(RecurringCommand RecurringCommand, IRecurringSchedule RecurringSchedule)>> FindAllRecurringCommandsAsync()
        {
            var recurringCommands = await this.queryProcessor.InGeneric(Context).ProcessAsync(new FindRecurringCommands(), MessageContext.CancellableContext(this.CancellationToken));
            var results = new List<(RecurringCommand, IRecurringSchedule)>();
            foreach (var recurringCommand in recurringCommands)
            {
                var format = (RecurringExpressionFormat)Enum.Parse(typeof(RecurringExpressionFormat), recurringCommand.RecurringExpressionFormat, ignoreCase: true);
                var recurringScheduleFactory = this.GetScheduleFactory(format);
                results.Add((recurringCommand, recurringScheduleFactory.Create(recurringCommand.RecurringExpression)));
            }  
            return results;
        }

        private IRecurringScheduleFactory GetScheduleFactory(RecurringExpressionFormat format) => this.recurringScheduleFactories.First(f => f.Format == format);

        private ITextSerializer GetSerializer(SerializationFormat format) => this.commandSerializers.First(s => s.Format == format);

        private async Task ManageCommandAsync(RecurringCommand recurringCommand, IRecurringSchedule recurringSchedule)
        {
            try
            {
                this.logger.LogInformation("Management of recurring command {CommandId} in the context '{Context}' has started.", recurringCommand.CommandId, this.Context.Name);
                var command = this.DeserializeCommand(recurringCommand);
                var now = SystemTime.Local();
                var nextOccurrence = recurringSchedule.GetNextOccurrence(now);
                while (nextOccurrence.HasValue)
                {
                    this.CancellationToken.ThrowIfCancellationRequested();
                    this.logger.LogDebug("Next processing of recurring command {CommandId} in the context '{Context}' is scheduled for {CommandExecutionTime}.", recurringCommand.CommandId, this.Context.Name, nextOccurrence);
                    bool success;
                    try
                    {
                        await this.commandProcessor.ProcessWithDelayAsync(command, nextOccurrence.Value - now, MessageContext.CancellableContext(this.CancellationToken));
                        success = true;
                    }
                    catch (Exception exception) when (!(exception is OperationCanceledException))
                    {
                        success = false;
                        this.logger.LogError(default, exception, "An error occurred while processing recurring command {CommandId} in the context '{Context}'.", recurringCommand.CommandId, this.Context.Name);
                        var failureCommand = new MarkRecurringCommandAsFailed
                        {
                            CommandId = recurringCommand.CommandId,
                            ExecutionTime = nextOccurrence.Value,
                            ExceptionInfo = exception.ToString()
                        };
                        await this.commandProcessor.InGeneric(Context).ProcessAsync(failureCommand);
                    }
                    if (success)
                    {
                        this.logger.LogDebug("Processing of recurring command {CommandId} in the context '{Context}' has successfully finished.", recurringCommand.CommandId, this.Context.Name);
                        var successCommand = new MarkRecurringCommandAsSuccessful
                        {
                            CommandId = recurringCommand.CommandId,
                            ExecutionTime = nextOccurrence.Value
                        };
                        await this.commandProcessor.InGeneric(Context).ProcessAsync(successCommand);
                    }
                    now = SystemTime.Local();
                    nextOccurrence = recurringSchedule.GetNextOccurrence(now);
                }
            }
            catch (OperationCanceledException)
            {
                this.logger.LogInformation("Management of recurring command {CommandId} in the context '{Context}' has stopped.", recurringCommand.CommandId, this.Context.Name);
                throw;
            }
            catch (Exception exception)
            {
                this.logger.LogError(default, exception, "An error occurred while managing recurring command {CommandId} in the context '{Context}'.", recurringCommand.CommandId, this.Context.Name);
            }
        }

        private async Task ManageCommandsAsync()
        {
            try
            {
                this.CancellationToken.ThrowIfCancellationRequested(); 
                await new SynchronizationContextRemover();
                var commandInfos = await this.FindAllRecurringCommandsAsync();
                var tasks = new List<Task>();
                foreach (var commandInfo in commandInfos)
                    tasks.Add(ManageCommandAsync(commandInfo.RecurringCommand, commandInfo.RecurringSchedule));
                await Task.WhenAll(tasks);
            }
            catch(OperationCanceledException)
            {
                this.logger.LogInformation("RecurringCommandManager for the context '{Context}' has stopped.", this.Context.Name);
            }
            catch (Exception exception)
            {
                this.logger.LogCritical(default, exception, "An error occurred while managing recurring commands in the context '{Context}'.", this.Context.Name);
            }
            finally
            {
                this.IsRunning = false;
            }
        }

        #endregion Methods

    }
}
