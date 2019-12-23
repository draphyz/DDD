using Xunit;
using System.Text;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Domain;
    using Core.Infrastructure.Serialization;
    using Infrastructure;
    using Mapping;

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

        protected override IObjectTranslator<IEvent, EventState> CreateEventTranslator()
        {
            return new EventTranslator(DataContractSerializerWrapper.Create(Encoding.Unicode));
        }

        #endregion Methods

    }
}
