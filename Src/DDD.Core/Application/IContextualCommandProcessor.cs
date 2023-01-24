using DDD.Core.Domain;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    /// <summary>
    /// Defines a component that processes generic commands in a specific bounded context.
    /// </summary>
    /// <remarks>
    /// This component is used to implement a generic mechanism to consume events and manage recurring commands in the different bounded contexts.
    /// </remarks>
    public interface IContextualCommandProcessor 
    {
        #region Properties

        /// <summary>
        /// The bounded context in which the command is processed.
        /// </summary>
        BoundedContext Context { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Processes synchronously a command of a specified type in the specific bounded context.
        /// </summary>
        void Process<TCommand>(TCommand command, IMessageContext context = null) where TCommand : class, ICommand;

        /// <summary>
        /// Processes asynchronously a command of a specified type in the specific bounded context.
        /// </summary>
        Task ProcessAsync<TCommand>(TCommand command, IMessageContext context = null) where TCommand : class, ICommand;

        #endregion Methods
    }
}
