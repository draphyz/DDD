using NHibernate.Mapping.ByCode.Conformist;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain.Prescriptions;

    internal class PrescribedPharmaceuticalProductMapping : SubclassMapping<PrescribedPharmaceuticalProduct>
    {

        public PrescribedPharmaceuticalProductMapping()
        {
            this.Lazy(false);
            // Fields
            this.DiscriminatorValue("Product");
        }

    }
}
