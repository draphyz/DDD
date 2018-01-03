using System.Collections.Generic;
using Dapper;
using System.Data;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Application.Prescriptions;
    using Core.Infrastructure.Data;

    public class PrescribedMedicationsByPrescriptionFinder : DbQueryHandler<FindPrescribedMedicationsByPrescription, IEnumerable<PrescribedMedicationDetails>>
    {

        #region Constructors

        public PrescribedMedicationsByPrescriptionFinder(IHealthcareConnectionFactory connectionFactory) 
            : base(connectionFactory)
        {
        }

        #endregion Constructors

        #region Methods

        protected override IEnumerable<PrescribedMedicationDetails> Execute(FindPrescribedMedicationsByPrescription query, IDbConnection connection)
        {
            var expressions = connection.Expressions();
            return connection.Query<PrescribedMedicationDetails>(SqlScripts.FindPrescribedMedicationsByPrescription.Replace("@", expressions.ParameterPrefix()),
                                                                 new { PrescriptionId = query.PrescriptionIdentifier } );
        }

        #endregion Methods

    }
}
