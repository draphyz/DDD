using Xunit;
using System.Text;
using System;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Domain;
    using Core.Infrastructure.Serialization;
    using Domain.Prescriptions;
    using Infrastructure.Prescriptions;
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

        #region Methods

        protected override IAsyncRepository<PharmaceuticalPrescription> CreateRepository()
        {
            return new PharmaceuticalPrescriptionRepository
            (
                new OracleHealthcareContext("Oracle"),
                new Domain.Prescriptions.BelgianPharmaceuticalPrescriptionTranslator(),
                new EventTranslator(DataContractSerializerWrapper.Create(new UTF8Encoding(false)))
            );
        }

        #endregion Methods

    }
}
