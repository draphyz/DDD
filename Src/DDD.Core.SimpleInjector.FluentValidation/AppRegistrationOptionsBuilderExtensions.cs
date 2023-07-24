using EnsureThat;
using FluentValidation;
using SimpleInjector;
using DDD.Validation;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Validation;

    public static class AppRegistrationOptionsBuilderExtensions
    {

        #region Methods

        public static AppRegistrationOptions.Builder<Container> ConfigureValidation(this AppRegistrationOptions.Builder<Container> builder)
        {
            Ensure.That(builder, nameof(builder)).IsNotNull();
            var extendableBuilder = (IExtendableRegistrationOptionsBuilder<AppRegistrationOptions, Container>)builder;
            var appOptions = extendableBuilder.Build();
            extendableBuilder.AddExtension(container => RegisterValidators(container, appOptions));
            return builder;
        }

        private static void RegisterValidators(Container container, AppRegistrationOptions options)
        {
            container.RegisterConditional(typeof(IValidator<>), options.AssembliesToScan, options.TypeFilter);
            container.Register(typeof(ISyncObjectValidator<>), typeof(FluentValidatorAdapter<>));
            container.Register(typeof(IAsyncObjectValidator<>), typeof(FluentValidatorAdapter<>));
        }

        #endregion Methods

    }
}
