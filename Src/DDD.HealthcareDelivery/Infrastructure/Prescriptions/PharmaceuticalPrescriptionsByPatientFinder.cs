using Conditions;
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

    public class PharmaceuticalPrescriptionsByPatientFinder 
        : IQueryHandler<FindPharmaceuticalPrescriptionsByPatient, IEnumerable<PharmaceuticalPrescriptionSummary>>
    {

        #region Fields

        private readonly IDbConnectionProvider<HealthcareDeliveryContext> connectionProvider;
        private readonly CompositeTranslator<Exception, QueryException> exceptionTranslator;

        #endregion Fields

        #region Constructors

        public PharmaceuticalPrescriptionsByPatientFinder(IDbConnectionProvider<HealthcareDeliveryContext> connectionProvider)
        {
            Condition.Requires(connectionProvider, nameof(connectionProvider)).IsNotNull();
            this.connectionProvider = connectionProvider;
            this.exceptionTranslator = new CompositeTranslator<Exception, QueryException>();
            this.exceptionTranslator.Register(new DbToQueryExceptionTranslator());
            this.exceptionTranslator.RegisterFallback();
        }

        #endregion Constructors

        #region Methods

        public IEnumerable<PharmaceuticalPrescriptionSummary> Handle(FindPharmaceuticalPrescriptionsByPatient query, IMessageContext context = null)
        {
            Condition.Requires(query, nameof(query)).IsNotNull();
            try
            {
                var connection = this.connectionProvider.GetOpenConnection();
                var expressions = connection.Expressions();
                return connection.Query<PharmaceuticalPrescriptionSummary>(SqlScripts.FindPharmaceuticalPrescriptionsByPatient.Replace("@", expressions.ParameterPrefix()),
                                                                           new { PatientId = query.PatientIdentifier });
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<QueryException>())
            {
                throw this.exceptionTranslator.Translate(ex, new { Query = query });
            }
        }

        public async Task<IEnumerable<PharmaceuticalPrescriptionSummary>> HandleAsync(FindPharmaceuticalPrescriptionsByPatient query, IMessageContext context = null)
        {
            Condition.Requires(query, nameof(query)).IsNotNull();
            try
            {
                await new SynchronizationContextRemover();
                var cancellationToken = context?.CancellationToken() ?? default;
                var connection = await this.connectionProvider.GetOpenConnectionAsync(cancellationToken);
                var expressions = connection.Expressions();
                return await connection.QueryAsync<PharmaceuticalPrescriptionSummary>
               (
                    new CommandDefinition
                    (
                        SqlScripts.FindPharmaceuticalPrescriptionsByPatient.Replace("@", expressions.ParameterPrefix()),
                        new { PatientId = query.PatientIdentifier },
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
