using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    [Collection("SqlServer")]
    public class SqlServerPrescribedMedicationsByPrescriptionFinderTests :
        PrescribedMedicationsByPrescriptionFinderTests<SqlServerFixture>
    {
        #region Constructors

        public SqlServerPrescribedMedicationsByPrescriptionFinderTests(SqlServerFixture fixture)
            : base(fixture)
        {
        }

        #endregion Constructors
    }
}