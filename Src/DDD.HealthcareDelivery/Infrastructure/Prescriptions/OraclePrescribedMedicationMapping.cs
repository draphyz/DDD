namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain.Prescriptions;

    internal class OraclePrescribedMedicationMapping<TMedicationCode> : PrescribedMedicationMapping<TMedicationCode>
        where TMedicationCode : MedicationCode
    {
        #region Constructors

        public OraclePrescribedMedicationMapping()
        {
            // Fields
            this.Discriminator(m => m.Column(m1 => m1.SqlType("varchar2(20)")));
        }

        #endregion Constructors
    }
}