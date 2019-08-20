using NHibernate.Mapping.ByCode.Conformist;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain.Prescriptions;

    internal class PrescribedPharmaceuticalCompoundingMapping : SubclassMapping<PrescribedPharmaceuticalCompounding>
    {

        public PrescribedPharmaceuticalCompoundingMapping()
        {
            this.Lazy(false);
            // Fields
            this.DiscriminatorValue("Compounding");
        }

    }
}
