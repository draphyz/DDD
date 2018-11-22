using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    /// <summary>
    /// Translates common language runtime methods to the equivalent sql standard expressions in a Oracle database (Version 11).
    /// </summary>
    /// <seealso cref="DbStandardExpressions" />
    public class Oracle11Expressions : DbStandardExpressions
    {

        #region Methods

        public override string FromDummy()
        {
            return "FROM DUAL";
        }

        public override string NextValue(string sequence, string schema = null)
        {
            Condition.Requires(sequence, nameof(sequence)).IsNotNullOrWhiteSpace();
            string expression = string.Empty;
            if (!string.IsNullOrWhiteSpace(schema))
                expression += $"{schema}.";
            expression += $"{sequence}.NEXTVAL";
            return expression;
        }

        public override string ParameterPrefix() => ":";

        #endregion Methods

    }
}