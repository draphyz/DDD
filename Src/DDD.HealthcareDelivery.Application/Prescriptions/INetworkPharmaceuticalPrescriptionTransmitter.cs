namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Domain.Prescriptions;

    public interface INetworkPharmaceuticalPrescriptionTransmitter
    {

        #region Methods

        ElectronicPrescriptionNumber Transmit(PharmaceuticalPrescriptionState prescription);

        #endregion Methods

    }
}