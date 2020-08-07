using Conditions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DDD.Core.Infrastructure.Data
{
    using Domain;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;

    public static class ModelBuilderExtensions
    {

        #region Methods

        public static ModelBuilder ApplyLowerCaseNamingConvention(this ModelBuilder modelBuilder)
        {
            Condition.Requires(modelBuilder, nameof(modelBuilder)).IsNotNull();
            return modelBuilder.ApplyNamingConvention(s => s.ToLowerInvariant());
        }

        public static ModelBuilder ApplyNamingConvention(this ModelBuilder modelBuilder, Func<string, string> namingConvention)
        {
            Condition.Requires(modelBuilder, nameof(modelBuilder)).IsNotNull();
            Condition.Requires(namingConvention, nameof(namingConvention)).IsNotNull();
            modelBuilder.BaseEntityTypes()
                        .Configure(e => e.SetTableName(namingConvention(e.GetTableName())));
            modelBuilder.PropertyTypes()
                        .Configure(p => p.SetColumnName(namingConvention(p.GetColumnName())));
            return modelBuilder;
        }

        public static ModelBuilder ApplyNonUnicodeStringsConvention(this ModelBuilder modelBuilder)
        {
            Condition.Requires(modelBuilder, nameof(modelBuilder)).IsNotNull();
            modelBuilder.PropertyTypes<string>()
                        .Configure(p => p.SetIsUnicode(false));
            return modelBuilder;
        }

        public static ModelBuilder ApplySnakeCaseNamingConvention(this ModelBuilder modelBuilder)
        {
            Condition.Requires(modelBuilder, nameof(modelBuilder)).IsNotNull();
            return modelBuilder.ApplyNamingConvention(s => s.ToSnakeCase());
        }

        public static ModelBuilder ApplyStateEntityConvention(this ModelBuilder modelBuilder)
        {
            Condition.Requires(modelBuilder, nameof(modelBuilder)).IsNotNull();
            modelBuilder.BaseEntityTypes<IStateEntity>()
                        .Configure(e => modelBuilder.Entity(e.ClrType).Ignore(nameof(IStateEntity.EntityState)));
            return modelBuilder;
        }

        public static ModelBuilder ApplyUpperCaseNamingConvention(this ModelBuilder modelBuilder)
        {
            Condition.Requires(modelBuilder, nameof(modelBuilder)).IsNotNull();
            return modelBuilder.ApplyNamingConvention(s => s.ToUpperInvariant());
        }

        public static ModelBuilder ApplyUpperSnakeCaseNamingConvention(this ModelBuilder modelBuilder)
        {
            Condition.Requires(modelBuilder, nameof(modelBuilder)).IsNotNull();
            return modelBuilder.ApplyNamingConvention(s => s.ToSnakeCase().ToUpperInvariant());
        }

        public static IEnumerable<IMutableEntityType> BaseEntityTypes(this ModelBuilder builder)
        {
            Condition.Requires(builder, nameof(builder)).IsNotNull();
            return builder.EntityTypes()
                          .Where(e => e.ClrType.BaseType == typeof(object));
        }

        public static IEnumerable<IMutableEntityType> BaseEntityTypes<TEntity>(this ModelBuilder modelBuilder)
        {
            Condition.Requires(modelBuilder, nameof(modelBuilder)).IsNotNull();
            return modelBuilder.BaseEntityTypes()
                               .Where(e => typeof(TEntity).IsAssignableFrom(e.ClrType));
        }

        public static IEnumerable<IMutableEntityType> EntityTypes(this ModelBuilder builder)
        {
            Condition.Requires(builder, nameof(builder)).IsNotNull();
            return builder.Model.GetEntityTypes();
        }

        public static IEnumerable<IMutableEntityType> EntityTypes<TEntity>(this ModelBuilder modelBuilder)
        {
            Condition.Requires(modelBuilder, nameof(modelBuilder)).IsNotNull();
            return modelBuilder.EntityTypes()
                               .Where(e => typeof(TEntity).IsAssignableFrom(e.ClrType));
        }

        public static IEnumerable<IMutableProperty> PropertyTypes(this ModelBuilder modelBuilder)
        {
            Condition.Requires(modelBuilder, nameof(modelBuilder)).IsNotNull();
            return modelBuilder.EntityTypes()
                               .SelectMany(e => e.GetProperties());
        }

        public static IEnumerable<IMutableProperty> PropertyTypes<TProperty>(this ModelBuilder modelBuilder)
        {
            Condition.Requires(modelBuilder, nameof(modelBuilder)).IsNotNull();
            return modelBuilder.PropertyTypes()
                               .Where(p => typeof(TProperty).IsAssignableFrom(p.ClrType));
        }

        #endregion Methods

    }
}