using System;
using System.Reflection;
using SimpleInjector;
using NHibernate.Cfg;
using Xunit;
using FluentAssertions;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Data;
    using Domain;

    public class ContainerExtensionsTests : IDisposable
    {

        #region Fields

        private readonly Container container;

        #endregion Fields

        #region Constructors

        public ContainerExtensionsTests()
        {
            this.container = new Container();
            this.container.Options.EnableAutoVerification = false;
        }

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
            this.container.Dispose();
        }

        [Fact]
        public void ConfigureApp_WithRegisterSessionFactoryFor_RegistersFactory()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.RegisterTypesFrom(Assembly.GetExecutingAssembly());
                o.RegisterSessionFactoryFor<FakeContext>(new Configuration());
            });
            // Assert
            var registration = container.GetRegistration<ISessionFactory<FakeContext>>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.Lifestyle == Lifestyle.Singleton);
            var sessionFactory = registration.GetInstance();
            sessionFactory.Should().BeOfType<DelegatingSessionFactory<FakeContext>>();
        }

        #endregion Methods

    }
}
