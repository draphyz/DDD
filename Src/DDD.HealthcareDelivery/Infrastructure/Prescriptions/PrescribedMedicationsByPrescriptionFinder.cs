using EnsureThat;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dapper;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain;
    using Application.Prescriptions;
    using Core.Application;
    using Core.Infrastructure.Data;
    using Mapping;
    using Threading;

    public class PrescribedMedicationsByPrescriptionFinder 
        : IQueryHandler<FindPrescribedMedicationsByPrescription, IEnumerable<PrescribedMedicationDetails>>
    {

        #region Fields

        private readonly IDbConnectionProvider<HealthcareDeliveryContext> connectionProvider;
        private readonly CompositeTranslator<Exception, QueryException> exceptionTranslator;

        #endregion Fields

        #region Constructors

        public PrescribedMedicationsByPrescriptionFinder(IDbConnectionProvider<HealthcareDeliveryContext> connectionProvider)
        {
            Ensure.That(connectionProvider, nameof(connectionProvider)).IsNotNull();
            this.connectionProvider = connectionProvider;
            this.exceptionTranslator = new CompositeTranslator<Exception, QueryException>();
            this.exceptionTranslator.Register(new DbToQueryExceptionTranslator());
            this.exceptionTranslator.RegisterFallback();
        }

        #endregion Constructors

        #region Methods

        public IEnumerable<PrescribedMedicationDetails> Handle(FindPrescribedMedicationsByPrescription query, IMessageContext context)
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            try
            {
                var connection = this.connectionProvider.GetOpenConnection();
                var expressions = connection.Expressions();
                return connection.Query<PrescribedMedicationDetails>(SqlScripts.FindPrescribedMedicationsByPrescription.Replace("@", expressions.ParameterPrefix()),
                                                                     new { PrescriptionId = query.PrescriptionIdentifier });
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<QueryException>())
            {
                throw this.exceptionTranslator.Translate(ex, new { Query = query });
            }
        }

        public async Task<IEnumerable<PrescribedMedicationDetails>> HandleAsync(FindPrescribedMedicationsByPrescription query, IMessageContext context)
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            Ensure.That(context, nameof(context)).IsNotNull();
            try
            {
                await new SynchronizationContextRemover();
                var cancellationToken = context.CancellationToken();
                var connection = await this.connectionProvider.GetOpenConnectionAsync(cancellationToken);
                var expressions = connection.Expressions();
                return await connection.QueryAsync<PrescribedMedicationDetails>
                (
                    new CommandDefinition
                    (
                        SqlScripts.FindPrescribedMedicationsByPrescription.Replace("@", expressions.ParameterPrefix()),
                        new { PrescriptionId = query.PrescriptionIdentifier },
                        cancellationToken: cancellationToken
                    )
                );
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<QueryException>())
            {
                throw this.exceptionTranslator.Translate(ex, new { Query = query });
            }
        }

        #endregion Methods

    }
}
