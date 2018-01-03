namespace DDD.Core.Infrastructure.Data
{
    public class FakeDbStandardExpressions : DbStandardExpressions
    {

        #region Methods

        public override string NextValue(string sequence, string schema = null)
        {
            return null;
        }

        #endregion Methods

    }
}