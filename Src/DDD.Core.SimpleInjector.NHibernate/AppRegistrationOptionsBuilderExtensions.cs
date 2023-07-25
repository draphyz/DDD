using EnsureThat;
using NHibernate;
using NHibernate.Cfg;
using SimpleInjector;
using System;
using System.Linq;
using System.Data.Common;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Domain;
    using Data;

    public static class AppRegistrationOptionsBuilderExtensions
    {

        #region Methods

        public static AppRegistrationOptions.Builder<Container> RegisterSessionFactoryFor<TContext>(this AppRegistrationOptions.Builder<Container> builder,
                                                                                                    Configuration configuration,
                                                                                                    Action<ISessionBuilder> options = null)
            where TContext : BoundedContext
        {
            Ensure.That(builder, nameof(builder)).IsNotNull();
            Ensure.That(configuration, nameof(configuration)).IsNotNull();
            options = options ?? (_ => { });
            var extendableBuilder = (IExtendableRegistrationOptionsBuilder<AppRegistrationOptions, Container>)builder;
            var appOptions = extendableBuilder.Build();
            extendableBuilder.AddExtension(container => RegisterSessionFactoryFor<TContext>(container, configuration, options));
            return builder;
        }

        private static void RegisterSessionFactoryFor<TContext>(Container container, Configuration configuration, Action<ISessionBuilder> options)
            where TContext : BoundedContext
        {
            container.RegisterSingleton<ISessionFactory<TContext>>(() =>
            {
                var context = container.GetInstance<TContext>();
                return new DelegatingSessionFactory<TContext>
                (
                    context,
                    configuration,
                    sessionBuilder =>
                    {
                        var connectionProvider = container.GetInstance<IDbConnectionProvider<TContext>>();
                        var connection = connectionProvider.GetOpenConnection();
                        sessionBuilder.Connection(connection);
                        options(sessionBuilder);
                    },
                    async (sessionBuilder, cancellationToken) =>
                    {
                        var connectionProvider = container.GetInstance<IDbConnectionProvider<TContext>>();
                        var connection = await connectionProvider.GetOpenConnectionAsync(cancellationToken);
                        sessionBuilder.Connection(connection);
                        options(sessionBuilder);
                    }
                );
            });
        }

        #endregion Methods

    }
}
