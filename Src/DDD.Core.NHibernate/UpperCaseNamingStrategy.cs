using NHibernate.Cfg;
using NHibernate.Util;

namespace DDD.Core.Infrastructure.Data
{
    public class UpperCaseNamingStrategy : INamingStrategy
    {
        #region Fields

        /// <summary>
        /// The singleton instance
        /// </summary>
        public static readonly INamingStrategy Instance = new UpperCaseNamingStrategy();

        #endregion Fields

        #region Constructors

        private UpperCaseNamingStrategy()
        {
        }

        #endregion Constructors

        #region Methods

        public string ClassToTableName(string className)
        {
            return StringHelper.Unqualify(className).ToUpperInvariant();
        }

        public string ColumnName(string columnName)
        {
            return columnName.ToUpperInvariant();
        }

        public string LogicalColumnName(string columnName, string propertyName)
        {
            return StringHelper.IsNotEmpty(columnName) ? columnName : StringHelper.Unqualify(propertyName);
        }

        public string PropertyToColumnName(string propertyName)
        {
            return StringHelper.Unqualify(propertyName).ToUpperInvariant();
        }

        public string PropertyToTableName(string className, string propertyName)
        {
            return StringHelper.Unqualify(propertyName).ToUpperInvariant();
        }

        public string TableName(string tableName)
        {
            return tableName.ToUpperInvariant();
        }

        #endregion Methods

    }
}
