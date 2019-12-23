using Xunit;
using System.Text;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Domain;
    using Core.Infrastructure.Serialization;
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

        #region Methods

        protected override HealthcareContext CreateContext()
        {
            return new SqlServerHealthcareContext("SqlServer");
        }

        protected override EventTranslator CreateEventTranslator()
        {
            return new EventTranslator(DataContractSerializerWrapper.Create(Encoding.Unicode));
        }

        #endregion Methods

    }
}
