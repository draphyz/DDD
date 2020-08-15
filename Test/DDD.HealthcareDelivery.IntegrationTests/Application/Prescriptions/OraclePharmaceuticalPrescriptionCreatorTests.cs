using Xunit;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Infrastructure;

    [Collection("Oracle")]
    public class OraclePharmaceuticalPrescriptionCreatorTests
        : PharmaceuticalPrescriptionCreatorTests<OracleFixture>
    {

        #region Constructors

        public OraclePharmaceuticalPrescriptionCreatorTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

    }
}
