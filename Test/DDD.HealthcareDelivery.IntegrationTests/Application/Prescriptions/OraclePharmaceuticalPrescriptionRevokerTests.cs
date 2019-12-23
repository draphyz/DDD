using Xunit;
using System.Text;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Domain;
    using Core.Infrastructure.Serialization;
    using Infrastructure;
    using Mapping;

    [Collection("Oracle")]
    public class OraclePharmaceuticalPrescriptionRevokerTests
        : PharmaceuticalPrescriptionRevokerTests<OracleFixture>
    {

        #region Constructors

        public OraclePharmaceuticalPrescriptionRevokerTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

        #region Methods

        protected override HealthcareContext CreateContext()
        {
            return new OracleHealthcareContext("Oracle");
        }

        protected override IObjectTranslator<IEvent, EventState> CreateEventTranslator()
        {
            return new EventTranslator(DataContractSerializerWrapper.Create(new UTF8Encoding(false)));
        }

        #endregion Methods

    }
}
