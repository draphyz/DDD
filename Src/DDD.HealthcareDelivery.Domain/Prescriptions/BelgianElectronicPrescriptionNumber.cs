namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    /// <summary>
    /// Represents the Belgian electronic prescription number (RID).
    /// </summary>
    public class BelgianElectronicPrescriptionNumber : ElectronicPrescriptionNumber
    {
        #region Constructors

        public BelgianElectronicPrescriptionNumber(string number) : base(number)
        {
        }

        #endregion Constructors
    }
}
