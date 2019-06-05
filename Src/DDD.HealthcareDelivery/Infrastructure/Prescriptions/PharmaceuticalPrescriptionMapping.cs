using NHibernate.Mapping.ByCode.Conformist;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain.Prescriptions;

    internal class PharmaceuticalPrescriptionMapping : SubclassMapping<PharmaceuticalPrescription>
    {
        public PharmaceuticalPrescriptionMapping()
        {
            this.Lazy(false);
            // Fields
            this.DiscriminatorValue("PHARM");
        }
    }
}
