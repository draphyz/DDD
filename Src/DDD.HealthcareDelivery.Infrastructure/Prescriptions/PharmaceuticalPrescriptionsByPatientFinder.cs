using System.Collections.Generic;
using Dapper;
using System.Data;
using System.Linq;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Application.Prescriptions;
    using Core.Infrastructure.Data;

    public class PharmaceuticalPrescriptionsByPatientFinder : DbQueryHandler<FindPharmaceuticalPrescriptionsByPatient, IEnumerable<PharmaceuticalPrescriptionSummary>>
    {

        #region Constructors

        public PharmaceuticalPrescriptionsByPatientFinder(IHealthcareConnectionFactory connectionFactory) 
            : base(connectionFactory)
        {
        }

        protected override IEnumerable<PharmaceuticalPrescriptionSummary> Execute(FindPharmaceuticalPrescriptionsByPatient query, IDbConnection connection)
        {
            var expressions = connection.Expressions();
            return connection.Query<PharmaceuticalPrescriptionSummary>(SqlScripts.FindPharmaceuticalPrescriptionsByPatient.Replace("@", expressions.ParameterPrefix()),
                                                                       new { PatientId = query.PatientIdentifier })
                             .ToList();
        }

        #endregion Constructors

    }
}
