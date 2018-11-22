using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    [Collection("Oracle")]
    public class OraclePrescribedMedicationsByPrescriptionFinderTests :
        PrescribedMedicationsByPrescriptionFinderTests<OracleFixture>
    {
        #region Constructors

        public OraclePrescribedMedicationsByPrescriptionFinderTests(OracleFixture fixture)
            : base(fixture)
        {
        }

        #endregion Constructors
    }
}