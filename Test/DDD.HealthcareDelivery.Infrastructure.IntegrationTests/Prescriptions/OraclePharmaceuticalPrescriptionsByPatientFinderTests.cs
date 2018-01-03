using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    [Collection("Oracle")]
    public class OraclePharmaceuticalPrescriptionsByPatientFinderTests : PharmaceuticalPrescriptionsByPatientFinderTests<OracleFixture>
    {

        #region Constructors

        public OraclePharmaceuticalPrescriptionsByPatientFinderTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

    }
}