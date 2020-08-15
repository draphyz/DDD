using System.Collections.Generic;
using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Application.Prescriptions;
    using Core.Infrastructure.Data;

    public class PrescribedMedicationsByPrescriptionFinder 
        : DbQueryHandler<FindPrescribedMedicationsByPrescription, IEnumerable<PrescribedMedicationDetails>>
    {

        #region Constructors

        public PrescribedMedicationsByPrescriptionFinder(IHealthcareDeliveryConnectionFactory connectionFactory) 
            : base(connectionFactory)
        {
        }

        #endregion Constructors

        #region Methods

        protected override Task<IEnumerable<PrescribedMedicationDetails>> ExecuteAsync(FindPrescribedMedicationsByPrescription query, 
                                                                            IDbConnection connection)
        {
            var expressions = connection.Expressions();
            return connection.QueryAsync<PrescribedMedicationDetails>
            (
                SqlScripts.FindPrescribedMedicationsByPrescription.Replace("@", expressions.ParameterPrefix()),
                new { PrescriptionId = query.PrescriptionIdentifier } 
            );
        }

        #endregion Methods

    }
}
