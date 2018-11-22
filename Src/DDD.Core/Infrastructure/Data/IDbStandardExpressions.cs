namespace DDD.Core.Infrastructure.Data
{
    /// <summary>
    /// Defines common language runtime methods that are translated to the corresponding standard sql expression.
    /// </summary>
    public interface IDbStandardExpressions
    {

        #region Methods

        string FromDummy();

        string NextValue(string sequence, string schema = null);

        string ParameterPrefix();

        #endregion Methods

    }
}