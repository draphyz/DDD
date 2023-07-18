using EnsureThat;
using System;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Mapping;
    
    public class MappingRegistrationOptions
    {

        #region Constructors

        private MappingRegistrationOptions() { }

        #endregion Constructors

        #region Properties

        public IObjectMapper<object, object> DefaultMapper { get; private set; }

        public IObjectTranslator<object, object> DefaultTranslator { get; private set; }

        #endregion Properties

        #region Classes

        public class Builder : FluentBuilder<MappingRegistrationOptions>
        {

            #region Fields

            private readonly MappingRegistrationOptions options = new MappingRegistrationOptions();

            #endregion Fields

            #region Methods

            public Builder RegisterDefaultTranslator(Func<object, IMappingContext, object> Translator)
            {
                Ensure.That(Translator, nameof(Translator)).IsNotNull();
                this.options.DefaultTranslator = new DelegatingTranslator<object, object>(Translator);
                return this;
            }

            public Builder RegisterDefaultMapper(Action<object, object, IMappingContext> mapper)
            {
                Ensure.That(mapper, nameof(mapper)).IsNotNull();
                this.options.DefaultMapper = new DelegatingMapper<object, object>(mapper);
                return this;
            }

            protected override MappingRegistrationOptions Build() => this.options;

            #endregion Methods

        }

        #endregion Classes

    }
}
