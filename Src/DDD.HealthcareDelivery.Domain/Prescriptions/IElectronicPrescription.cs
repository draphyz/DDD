namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    public interface IElectronicPrescription
    {

        #region Methods

        void Send(ElectronicPrescriptionNumber number);

        #endregion Methods

    }
}
