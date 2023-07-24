using System;
using SimpleInjector;
using Microsoft.EntityFrameworkCore;
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
        public void ConfigureApp_WithRegisterDbContextFactory_RegistersFactory()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.ConfigureDbConnectionFor<FakeContext>(oc =>
                {
                    oc.SetProviderName("FakeProviderName");
                    oc.SetConnectionString("FakeConnectionString");
                });
                o.RegisterDbContextFactory<FakeDbContext, FakeContext>((oc, c) =>
                {
                    oc.UseSqlServer(c);
                    return new FakeDbContext(oc.Options);
                });
            });
            // Assert
            var registration = container.GetRegistration<IDbContextFactory<FakeDbContext>>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.Lifestyle == Lifestyle.Singleton);
            var dbContextFactory = registration.GetInstance();
            dbContextFactory.Should().BeOfType<DelegatingDbContextFactory<FakeDbContext>>();
        }

        #endregion Methods

    }
}
