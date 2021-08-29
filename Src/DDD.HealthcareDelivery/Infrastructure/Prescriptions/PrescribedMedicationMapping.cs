using NHibernate.Mapping.ByCode.Conformist;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain.Prescriptions;
    using NHibernate;
    using NHibernate.Mapping.ByCode;

    internal abstract class PrescribedMedicationMapping<TMedicationCode> : ClassMapping<PrescribedMedication>
        where TMedicationCode : MedicationCode
    {

        #region Constructors

        protected PrescribedMedicationMapping()
        {
            this.Lazy(false);
            // Table
            this.Table("PrescMedication");
            // Keys
            this.Id("identifier", m =>
            {
                m.Column("PrescMedicationId");
                m.Generator(Generators.Sequence, m1 => m1.Params(new { sequence = "PrescMedicationId" }));
            });
            // Fields
            this.Discriminator(m =>
            {
                m.Column("MedicationType");
                m.Length(20);
                m.NotNullable(true);
            });
            this.Property(med => med.NameOrDescription, m =>
            {
                m.Column("NameOrDesc");
                m.Type(NHibernateUtil.AnsiString);
                m.Length(1024);
                m.NotNullable(true);
            });
            this.Property(med => med.Posology, m =>
            {
                m.Type(NHibernateUtil.AnsiString);
                m.Length(1024);
            });
            this.Property(med => med.Quantity);
            this.Component(med => med.Code, m =>
            {
                m.Class<TMedicationCode>();
                m.Property(c => c.Value, m1 =>
                {
                    m1.Column("Code");
                    m1.Type(NHibernateUtil.AnsiString);
                    m1.Length(20);
                });
            });
        }

        #endregion Constructors

    }
}
