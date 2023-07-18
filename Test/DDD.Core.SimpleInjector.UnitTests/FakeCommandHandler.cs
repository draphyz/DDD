namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Application;
    using System.Threading.Tasks;

    public class FakeCommandHandler : ICommandHandler<FakeCommand>
    {
        #region Methods

        public void Handle(FakeCommand command, IMessageContext context)
        {
        }

        public Task HandleAsync(FakeCommand command, IMessageContext context)
        {
            return Task.CompletedTask;
        }

        #endregion Methods
    }
}
