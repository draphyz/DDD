using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;
using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    /// <summary>
    /// Make all table names upper case. Default is to use snake case too.
    /// </summary>
    public class UpperCaseTableNameConvention : IStoreModelConvention<EntitySet>
    {

        #region Fields

        private readonly bool useSnakeCase = true;

        #endregion Fields

        #region Constructors

        public UpperCaseTableNameConvention(bool useSnakeCase = true)
        {
            this.useSnakeCase = useSnakeCase;
        }

        #endregion Constructors

        #region Methods

        public void Apply(EntitySet item, DbModel model)
        {
            Condition.Requires(item, nameof(item)).IsNotNull();
            Condition.Requires(model, nameof(model)).IsNotNull();
            var tableName = item.Table;
            if (useSnakeCase)
                tableName = tableName.ToSnakeCase();
            item.Table = tableName.ToUpperInvariant();
        }

        #endregion Methods

    }
}
