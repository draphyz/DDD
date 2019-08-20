using NHibernate.Mapping.ByCode.Conformist;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain.Prescriptions;
    using NHibernate;
    using NHibernate.Mapping.ByCode;

    internal abstract class PrescribedMedicationMapping<TMedicationCode> : ClassMapping<PrescribedMedication>
        where TMedicationCode : MedicationCode
    {

        #region Fields

        private readonly bool useUpperCase;

        #endregion Fields

        #region Constructors

        protected PrescribedMedicationMapping(bool useUpperCase)
        {
            this.useUpperCase = useUpperCase;
            this.Lazy(false);
            // Table
            this.Table(ToCasingConvention("PrescMedication"));
            // Keys
            this.Id("identifier", m =>
            {
                m.Column(ToCasingConvention("PrescMedicationId"));
                m.Generator(Generators.Sequence, m1 => m1.Params(new { sequence = ToCasingConvention("PrescMedicationId") }));
            });
            // Fields
            this.Discriminator(m =>
            {
                m.Column(ToCasingConvention("MedicationType"));
                m.Length(20);
                m.NotNullable(true);
            });
            this.Property(med => med.NameOrDescription, m =>
            {
                m.Column(ToCasingConvention("NameOrDesc"));
                m.Type(NHibernateUtil.AnsiString);
                m.Length(1024);
                m.NotNullable(true);
            });
            this.Property(med => med.Posology, m =>
            {
                m.Type(NHibernateUtil.AnsiString);
                m.Length(1024);
            });
            this.Property(med => med.Quantity, m =>
            {
                m.Type(NHibernateUtil.AnsiString);
                m.Length(100);
            });
            this.Property(med => med.Duration, m =>
            {
                m.Type(NHibernateUtil.AnsiString);
                m.Length(100);
            });
            this.Component(med => med.Code, m =>
            {
                m.Class<TMedicationCode>();
                m.Property(c => c.Value, m1 =>
                {
                    m1.Column(ToCasingConvention("Code"));
                    m1.Type(NHibernateUtil.AnsiString);
                    m1.Length(20);
                });
            });
        }

        #endregion Constructors

        #region Methods

        protected string ToCasingConvention(string name) => this.useUpperCase ? name.ToUpperInvariant() : name;

        #endregion Methods

    }
}
