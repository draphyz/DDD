using System;
using SimpleInjector;
using EnsureThat;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    public class RegisteredType
    {

        #region Constructors

        public RegisteredType(Type type, Lifestyle lifestyle) 
        { 
            Ensure.That(type, nameof(type)).IsNotNull();
            Ensure.That(lifestyle, nameof(lifestyle)).IsNotNull();
            this.Type = type;
            this.Lifestyle = lifestyle;
        }

        #endregion Constructors

        #region Properties

        public Type Type { get; }

        public Lifestyle Lifestyle { get; }

        #endregion Properties

    }
}
