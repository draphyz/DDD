using System.Collections.Generic;
using Dapper;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Application.Prescriptions;
    using Core.Infrastructure.Data;

    public class PharmaceuticalPrescriptionsByPatientFinder 
        : DbQueryHandler<FindPharmaceuticalPrescriptionsByPatient, IEnumerable<PharmaceuticalPrescriptionSummary>>
    {

        #region Constructors

        public PharmaceuticalPrescriptionsByPatientFinder(IHealthcareDeliveryConnectionFactory connectionFactory) 
            : base(connectionFactory)
        {
        }

        protected override Task<IEnumerable<PharmaceuticalPrescriptionSummary>> ExecuteAsync(FindPharmaceuticalPrescriptionsByPatient query, 
                                                                                             IDbConnection connection,
                                                                                             CancellationToken cancellationToken = default)
        {
            var expressions = connection.Expressions();
            return connection.QueryAsync<PharmaceuticalPrescriptionSummary>
           (
                new CommandDefinition
                (
                    SqlScripts.FindPharmaceuticalPrescriptionsByPatient.Replace("@", expressions.ParameterPrefix()), 
                    new { PatientId = query.PatientIdentifier }, 
                    cancellationToken: cancellationToken
                )
            );
        }

        #endregion Constructors

    }
}
