using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain.Prescriptions;

    internal class PharmaceuticalPrescriptionMapping : SubclassMapping<PharmaceuticalPrescription>
    {

        #region Constructors

        public PharmaceuticalPrescriptionMapping()
        {
            this.Lazy(false);
            // Fields
            this.DiscriminatorValue("PHARM");
            // Associations
            this.Set<PrescribedMedication>("prescribedMedications",
            m =>
            {
                m.Key(m1 =>
                {
                    m1.Column("PrescriptionId");
                    m1.NotNullable(true);
                });
                m.Cascade(Cascade.All);
            },
            r => r.OneToMany(m => m.Class(typeof(PrescribedMedication))));
        }

        #endregion Constructors

    }
}
