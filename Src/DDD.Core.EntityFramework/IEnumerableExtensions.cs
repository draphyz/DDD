using EnsureThat;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;

namespace DDD.Core.Infrastructure.Data
{
    public static class IEnumerableExtensions
    {

        #region Methods

        public static void Configure(this IEnumerable<IMutableEntityType> entityTypes, Action<IMutableEntityType> buildAction)
        {
            Ensure.That(entityTypes, nameof(entityTypes)).IsNotNull();
            Ensure.That(buildAction, nameof(buildAction)).IsNotNull();
            foreach (var entityType in entityTypes)
                buildAction(entityType);
        }

        public static void Configure(this IEnumerable<IMutableProperty> propertyTypes, Action<IMutableProperty> buildAction)
        {
            Ensure.That(propertyTypes, nameof(propertyTypes)).IsNotNull();
            Ensure.That(buildAction, nameof(buildAction)).IsNotNull();
            foreach (var propertyType in propertyTypes)
                buildAction(propertyType);
        }

        #endregion Methods

    }
}
