namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Common.Domain;

    public abstract class MedicationCode : IdentificationCode
    {

        #region Constructors

        protected MedicationCode(string code) : base(code)
        {
        }

        #endregion Constructors

    }
}
