namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain.Prescriptions;

    internal class SqlServerPrescribedMedicationMapping<TMedicationCode> : PrescribedMedicationMapping<TMedicationCode>
        where TMedicationCode : MedicationCode
    {
        #region Constructors

        public SqlServerPrescribedMedicationMapping() 
        {
            // Fields
            this.Discriminator(m => m.Column(m1 => m1.SqlType("varchar(20)")));
        }

        #endregion Constructors
    }
}