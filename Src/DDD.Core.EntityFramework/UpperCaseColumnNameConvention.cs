using System;
using System.Reflection;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DDD.Core.Infrastructure.Data
{
    /// <summary>
    /// Make all column names upper case. Default is to use snake case too.
    /// </summary>
    public class UpperCaseColumnNameConvention : Convention
    {

        #region Fields

        private readonly bool useSnakeCase = true;
        private readonly Func<PropertyInfo, bool> isIgnoredProperty = p => false;

        #endregion Fields

        #region Constructors

        public UpperCaseColumnNameConvention(bool useSnakeCase = true, Func<PropertyInfo, bool> isIgnoredProperty = null)
        {
            this.useSnakeCase = useSnakeCase;
            if (isIgnoredProperty != null)
                this.isIgnoredProperty = isIgnoredProperty;
            this.Properties()
                .Where(p => !this.isIgnoredProperty(p))
                .Configure(c => c.HasColumnName(GetColumnName(c)));
        }

        #endregion Constructors

        #region Methods

        private string GetColumnName(ConventionPrimitivePropertyConfiguration type)
        {
            var columnName = type.ClrPropertyInfo.Name;
            if (useSnakeCase)
                columnName = columnName.ToSnakeCase();
            return columnName.ToUpperInvariant();
        }

        #endregion Methods

    }
}
