using Xunit;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Infrastructure;

    [Collection("SqlServer")]
    public class SqlServerPharmaceuticalPrescriptionCreatorTests
        : PharmaceuticalPrescriptionCreatorTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerPharmaceuticalPrescriptionCreatorTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

    }
}
