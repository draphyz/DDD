namespace DDD.Core.Infrastructure.Data
{
    /// <summary>
    /// Base class that translates common language runtime methods to the equivalent sql standard expressions.
    /// </summary>
    /// <seealso cref="IDbStandardExpressions" />
    public abstract class DbStandardExpressions : IDbStandardExpressions
    {

        #region Fields

        public readonly static IDbStandardExpressions Oracle11 = new Oracle11Expressions();

        public readonly static IDbStandardExpressions SqlServer2012 = new SqlServer2012Expressions();

        #endregion Fields

        #region Methods

        public virtual string FromDummy() => string.Empty;

        public abstract string NextValue(string sequence, string schema = null);

        public virtual string ParameterPrefix() => "@";

        #endregion Methods

    }
}