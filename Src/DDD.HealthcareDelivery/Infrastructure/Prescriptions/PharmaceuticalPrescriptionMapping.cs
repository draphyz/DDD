using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain.Prescriptions;

    internal abstract class PharmaceuticalPrescriptionMapping : SubclassMapping<PharmaceuticalPrescription>
    {

        #region Fields

        private readonly bool useUpperCase;

        #endregion Fields

        #region Constructors

        protected PharmaceuticalPrescriptionMapping(bool useUpperCase)
        {
            this.useUpperCase = useUpperCase;
            this.Lazy(false);
            // Fields
            this.DiscriminatorValue("PHARM");
            // Associations
            this.Set<PrescribedMedication>("prescribedMedications",
            m =>
            {
                m.Key(m1 =>
                {
                    m1.Column(ToCasingConvention("PrescriptionId"));
                    m1.NotNullable(true);
                });
                m.Cascade(Cascade.All);
            },
            r => r.OneToMany(m => m.Class(typeof(PrescribedMedication))));
        }

        #endregion Constructors

        #region Methods

        protected string ToCasingConvention(string name) => this.useUpperCase ? name.ToUpperInvariant() : name;

        #endregion Methods

    }
}
