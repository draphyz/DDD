using Conditions;
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
            Condition.Requires(entityTypes, nameof(entityTypes)).IsNotNull();
            Condition.Requires(buildAction, nameof(buildAction)).IsNotNull();
            foreach (var entityType in entityTypes)
                buildAction(entityType);
        }

        public static void Configure(this IEnumerable<IMutableProperty> propertyTypes, Action<IMutableProperty> buildAction)
        {
            Condition.Requires(propertyTypes, nameof(propertyTypes)).IsNotNull();
            Condition.Requires(buildAction, nameof(buildAction)).IsNotNull();
            foreach (var propertyType in propertyTypes)
                buildAction(propertyType);
        }

        #endregion Methods

    }
}
