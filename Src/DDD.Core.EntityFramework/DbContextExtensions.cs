using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using EnsureThat;

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
            Ensure.That(context, nameof(context)).IsNotNull();
            Ensure.That(entityType, nameof(entityType)).IsNotNull();
            return context.Model.FindEntityType(entityType).FindPrimaryKey().Properties.Select(p => p.Name);
        }

        #endregion Methods

    }
}