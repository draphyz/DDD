namespace DDD.Core.Infrastructure.Testing
{
    using Data;

    public interface IDbFixture<out TConnectionFactory> 
        where TConnectionFactory : class, IDbConnectionFactory
    {

        #region Properties

        TConnectionFactory ConnectionFactory { get; }

        #endregion Properties

        #region Methods

        int[] ExecuteScript(string script);

        int[] ExecuteScriptFromResources(string scriptName);

        #endregion Methods

    }
}