using EnsureThat;
using System;

namespace DDD.Core.Domain
{
    using Mapping;
    using Collections;

    public static class CompositeTranslatorExtensions
    {

        #region Methods

        public static void RegisterFallback(this CompositeTranslator<Exception, RepositoryException> translator)
        {
            Ensure.That(translator, nameof(translator)).IsNotNull();
            translator.Register<Exception>((exception, context) =>
            {
                Type entityType = null;
                context?.TryGetValue("EntityType", out entityType);
                return new RepositoryException(isTransient: false, entityType, exception);
            });
        }

        public static void RegisterFallback(this CompositeTranslator<Exception, DomainServiceException> translator)
        {
            Ensure.That(translator, nameof(translator)).IsNotNull();
            translator.Register<Exception>((exception, context) =>
            {
                Type serviceType = null;
                context?.TryGetValue("ServiceType", out serviceType);
                return new DomainServiceException(isTransient: false, serviceType, exception);
            });
        }

        #endregion Methods

    }
}