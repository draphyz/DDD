using System.Data.Common;

namespace DDD.Core.Infrastructure.Testing
{
    using Data;
    using Domain;

    public interface IDbFixture<TContext>
        where TContext : BoundedContext
    {

        #region Methods

        IDbConnectionProvider<TContext> CreateConnectionProvider(bool pooling = true);

        DbConnection CreateConnection(bool pooling = true);

        int[] ExecuteScript(string script);

        int[] ExecuteScriptFromResources(string scriptName);

        #endregion Methods

    }
}