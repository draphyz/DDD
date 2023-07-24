using EnsureThat;
using Microsoft.EntityFrameworkCore;
using SimpleInjector;
using System.Data.Common;
using System;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Domain;
    using Data;

    public static class AppRegistrationOptionsBuilderExtensions
    {
        #region Methods

        public static AppRegistrationOptions.Builder<Container> RegisterDbContextFactory<TDbContext, TContext>(this AppRegistrationOptions.Builder<Container> builder, 
                                                                                                               Func<DbContextOptionsBuilder, DbConnection, TDbContext> factory)
            where TDbContext : DbContext
            where TContext : BoundedContext
        {
            Ensure.That(builder, nameof(builder)).IsNotNull();
            Ensure.That(factory, nameof(factory)).IsNotNull();
            var extendableBuilder = (IExtendableRegistrationOptionsBuilder<AppRegistrationOptions, Container>)builder;
            extendableBuilder.AddExtension(container => RegisterDbContextFactory<TDbContext, TContext>(container, factory));
            return builder;
        }


        private static void RegisterDbContextFactory<TDbContext, TContext>(Container container, 
                                                                           Func<DbContextOptionsBuilder, DbConnection, TDbContext> factory)
            where TDbContext : DbContext
            where TContext : BoundedContext
        {
            container.RegisterSingleton<IDbContextFactory<TDbContext>>(() =>
            new DelegatingDbContextFactory<TDbContext>
            (
                optionsBuilder =>
                {
                    var connectionProvider = container.GetInstance<IDbConnectionProvider<TContext>>();
                    var connection = connectionProvider.GetOpenConnection();
                    return factory(optionsBuilder, connection);
                },
                async (optionsBuilder, cancellationToken) =>
                {
                    var connectionProvider = container.GetInstance<IDbConnectionProvider<TContext>>();
                    var connection = await connectionProvider.GetOpenConnectionAsync(cancellationToken);
                    return factory(optionsBuilder, connection);
                }
            ));
        }

        #endregion Methods


    }
}
