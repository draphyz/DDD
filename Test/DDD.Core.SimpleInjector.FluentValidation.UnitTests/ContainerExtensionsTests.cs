using System;
using FluentValidation;
using FluentAssertions;
using System.Reflection;
using SimpleInjector;
using Xunit;
using DDD.Validation;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Validation;

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
        public void ConfigureApp_WithConfigureValidation_RegistersValidators()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.RegisterTypesFrom(Assembly.GetExecutingAssembly());
                o.ConfigureValidation();
            });
            // Assert
            var registration = container.GetRegistration<IValidator<FakeObject>>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.ImplementationType == typeof(FakeObjectValidator)
                                                                            && r.Lifestyle == Lifestyle.Transient);
        }

        [Fact]
        public void ConfigureApp_WithConfigureValidation_RegistersSyncObjectValidators()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.RegisterTypesFrom(Assembly.GetExecutingAssembly());
                o.ConfigureValidation();
            });
            // Assert
            var registration = container.GetRegistration<ISyncObjectValidator<FakeObject>>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.ImplementationType == typeof(FluentValidatorAdapter<FakeObject>)
                                                                            && r.Lifestyle == Lifestyle.Transient);
        }

        [Fact]
        public void ConfigureApp_WithConfigureValidation_RegistersAsyncObjectValidators()
        {
            // Act
            this.container.ConfigureApp(o =>
            {
                o.RegisterTypesFrom(Assembly.GetExecutingAssembly());
                o.ConfigureValidation();
            });
            // Assert
            var registration = container.GetRegistration<IAsyncObjectValidator<FakeObject>>();
            registration.Should().NotBeNull().And.Match<InstanceProducer>(r => r.ImplementationType == typeof(FluentValidatorAdapter<FakeObject>)
                                                                            && r.Lifestyle == Lifestyle.Transient);
        }

        #endregion Methods

    }
}
