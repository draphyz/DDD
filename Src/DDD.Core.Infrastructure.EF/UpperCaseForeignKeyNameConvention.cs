using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    /// <summary>
    /// Make all foreign key names upper case. Default is to use snake case too.
    /// </summary>
    public class UpperCaseForeignKeyNameConvention : IStoreModelConvention<AssociationType>
    {
        #region Fields

        private readonly bool useSnakeCase = true;

        #endregion Fields

        #region Constructors

        public UpperCaseForeignKeyNameConvention(bool useSnakeCase = true)
        {
            this.useSnakeCase = useSnakeCase;
        }

        #endregion Constructors

        #region Methods

        public void Apply(AssociationType association, DbModel model)
        {
            Condition.Requires(association, nameof(association)).IsNotNull();
            Condition.Requires(model, nameof(model)).IsNotNull();
            if (association.IsForeignKey)
                UpperCaseForeignKeyProperties(association.Constraint.ToProperties);
        }

        private void UpperCaseForeignKeyProperties(IEnumerable<EdmProperty> properties)
        {
            foreach (var property in properties)
            {
                var foreignKeyName = property.Name;
                if (useSnakeCase)
                    foreignKeyName = foreignKeyName.ToSnakeCase();
                property.Name = foreignKeyName.ToUpperInvariant();
            }
        }

        #endregion Methods
    }
}
