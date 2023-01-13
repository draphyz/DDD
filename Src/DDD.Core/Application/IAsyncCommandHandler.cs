using System.Threading.Tasks;

namespace DDD.Core.Application
{
    /// <summary>
    /// Defines a method that handles asynchronously a command of a specified type.
    /// </summary>
    public interface IAsyncCommandHandler<in TCommand>
        where TCommand : class, ICommand
    {

        #region Methods

        /// <summary>
        /// Handles asynchronously a command of a specified type.
        /// </summary>
        Task HandleAsync(TCommand command, IMessageContext context = null);

        #endregion Methods

    }
}