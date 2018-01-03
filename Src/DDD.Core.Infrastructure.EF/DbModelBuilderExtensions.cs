using System;
using System.Reflection;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DDD.Core.Infrastructure.Data
{
    public static class DbModelBuilderExtensions
    {

        #region Methods

        /// <summary>
        /// Add custom upper case conventions to ModelBuilder instance. Default is to use snake case too.
        /// </summary>
        /// <param name="modelBuilder">Entity Framework DbModelBuilder instance.</param>
        /// <param name="useSnakeCase">Use snake case.</param>
        /// <param name="isIgnoredProperty">Returns a value indicating whether the specified property is excluded from the model.</param>
        public static void ApplyAllUpperCaseConventions(this DbModelBuilder modelBuilder, bool useSnakeCase = true, Func<PropertyInfo, bool> isIgnoredProperty = null)
        {

            IConvention[] conventions =
            {
                    new UpperCaseTableNameConvention(useSnakeCase),
                    new UpperCaseForeignKeyNameConvention(useSnakeCase),
                    new UpperCaseColumnNameConvention(useSnakeCase, isIgnoredProperty)
            };
            modelBuilder.Conventions.Add(conventions);
        }

        #endregion Methods

    }
}
