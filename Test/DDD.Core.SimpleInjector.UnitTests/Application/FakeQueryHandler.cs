using System.Threading.Tasks;

namespace DDD.Core.Application
{
    public class FakeQueryHandler : IQueryHandler<FakeQuery, string>
    {
        #region Methods

        public string Handle(FakeQuery query, IMessageContext context)
        {
            return null;
        }

        public Task<string> HandleAsync(FakeQuery query, IMessageContext context)
        {
            return Task.FromResult<string>(null);
        }

        #endregion Methods
    }
}