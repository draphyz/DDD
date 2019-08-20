using NHibernate.Mapping.ByCode.Conformist;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain.Prescriptions;

    internal class PrescribedPharmaceuticalSubstanceMapping : SubclassMapping<PrescribedPharmaceuticalSubstance>
    {

        public PrescribedPharmaceuticalSubstanceMapping()
        {
            this.Lazy(false);
            // Fields
            this.DiscriminatorValue("Substance");
        }

    }
}
