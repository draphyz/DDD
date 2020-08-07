using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    public static class DbContextExtensions
    {

        #region Methods

        public static IEnumerable<string> GetKeyNames<TEntity>(this DbContext context)
            where TEntity : class
        {
            return context.GetKeyNames(typeof(TEntity));
        }

        public static IEnumerable<string> GetKeyNames(this DbContext context, Type entityType)
        {
            Condition.Requires(context, nameof(context)).IsNotNull();
            Condition.Requires(entityType, nameof(entityType)).IsNotNull();
            return context.Model.FindEntityType(entityType).FindPrimaryKey().Properties.Select(p => p.Name);
        }

        #endregion Methods

    }
}