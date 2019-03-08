namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Common.Domain;

    /// <summary>
    ///  Represents a code or a number that identifies a unique product or medication.
    /// </summary>
    public abstract class MedicationCode : IdentificationCode
    {

        #region Constructors

        protected MedicationCode(string code) : base(code)
        {
        }

        #endregion Constructors

    }
}
