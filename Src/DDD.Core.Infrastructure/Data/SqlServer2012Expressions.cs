using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    /// <summary>
    /// Translates common language runtime methods to the equivalent standard sql expressions in a Sql Server database (Version 2012).
    /// </summary>
    /// <seealso cref="DbStandardExpressions" />
    public class SqlServer2012Expressions : DbStandardExpressions
    {
        #region Methods

        public override string NextValue(string sequence, string schema = null)
        {
            Condition.Requires(sequence, nameof(sequence)).IsNotNullOrWhiteSpace();
            var expression = "NEXT VALUE FOR ";
            if (!string.IsNullOrWhiteSpace(schema))
                expression += $"{schema}.";
            expression += sequence;
            return expression;
        }

        #endregion Methods
    }
}