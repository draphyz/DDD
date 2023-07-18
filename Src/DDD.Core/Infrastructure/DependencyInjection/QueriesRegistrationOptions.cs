using EnsureThat;
using System;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Application;
    using Validation;

    public class QueriesRegistrationOptions
    {

        #region Constructors

        private QueriesRegistrationOptions() { }

        #endregion Constructors

        #region Properties

        public IObjectValidator<IQuery> DefaultValidator { get; private set; }

        #endregion Properties

        #region Classes

        public class Builder : FluentBuilder<QueriesRegistrationOptions>
        {

            #region Fields

            private readonly QueriesRegistrationOptions options = new QueriesRegistrationOptions();

            #endregion Fields

            #region Methods

            public Builder RegisterDefaultValidator(Func<IQuery, IValidationContext, ValidationResult> validator)
            {
                Ensure.That(validator, nameof(validator)).IsNotNull();
                this.options.DefaultValidator = DelegatingValidator<IQuery>.Create(validator);
                return this;
            }

            public Builder RegisterDefaultSuccessfullyValidator()
            {
                Func<IQuery, IValidationContext, ValidationResult> validator = (query, context)
                    => new ValidationResult(true, query.GetType().Name, new ValidationFailure[] { });
                return this.RegisterDefaultValidator(validator);
            }

            protected override QueriesRegistrationOptions Build() => this.options;

            #endregion Methods

        }

        #endregion Classes

    }
}
