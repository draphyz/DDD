using System.Threading;
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

        Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);

        #endregion Methods

    }
}