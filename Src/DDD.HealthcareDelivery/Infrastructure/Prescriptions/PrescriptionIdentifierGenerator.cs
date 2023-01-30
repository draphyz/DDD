using EnsureThat;
using System;
using System.Threading.Tasks;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain;
    using Application.Prescriptions;
    using Core.Application;
    using Core.Infrastructure.Data;
    using DDD;
    using Mapping;
    using Threading;

    public class PrescriptionIdentifierGenerator : IQueryHandler<GeneratePrescriptionIdentifier, int>
    {

        #region Fields

        private readonly IDbConnectionProvider<HealthcareDeliveryContext> connectionProvider;
        private readonly CompositeTranslator<Exception, QueryException> exceptionTranslator;

        #endregion Fields

        #region Constructors

        public PrescriptionIdentifierGenerator(IDbConnectionProvider<HealthcareDeliveryContext> connectionProvider)
        {
            Ensure.That(connectionProvider, nameof(connectionProvider)).IsNotNull();
            this.connectionProvider = connectionProvider;
            this.exceptionTranslator = new CompositeTranslator<Exception, QueryException>();
            this.exceptionTranslator.Register(new DbToQueryExceptionTranslator());
            this.exceptionTranslator.RegisterFallback();
        }

        #endregion Constructors

        #region Methods

        public int Handle(GeneratePrescriptionIdentifier query, IMessageContext context = null)
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            try
            {
                var connection = this.connectionProvider.GetOpenConnection();
                return connection.NextValue<int>("PrescriptionId");
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<QueryException>())
            {
                throw this.exceptionTranslator.Translate(ex, new { Query = query });
            }
        }

        public async Task<int> HandleAsync(GeneratePrescriptionIdentifier query, IMessageContext context = null)
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            try
            {
                await new SynchronizationContextRemover();
                var cancellationToken = context?.CancellationToken() ?? default;
                var connection = await this.connectionProvider.GetOpenConnectionAsync(cancellationToken);
                return await connection.NextValueAsync<int>("PrescriptionId", null, cancellationToken);
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<QueryException>())
            {
                throw this.exceptionTranslator.Translate(ex, new { Query = query });
            }
        }

        #endregion Methods

    }
}
