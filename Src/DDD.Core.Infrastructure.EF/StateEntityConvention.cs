using System;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection;

namespace DDD.Core.Infrastructure.Data
{
    using Domain;

    public class StateEntityConvention : Convention
    {
        #region Constructors

        public StateEntityConvention()
        {
            this.Types<IStateEntity>().Where(t => Condition(t)).Configure(c => c.Ignore(e => e.EntityState));
        }

        #endregion Constructors

        #region Methods

        private static bool Condition(Type t)
        {
            return t.GetProperty(nameof(IStateEntity.EntityState), BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance) != null;
        }

        #endregion Methods
    }
}