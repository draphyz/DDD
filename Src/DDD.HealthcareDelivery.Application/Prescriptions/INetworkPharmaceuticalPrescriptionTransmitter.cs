namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Domain.Prescriptions;

    public interface INetworkPharmaceuticalPrescriptionTransmitter
    {

        #region Methods

        ElectronicPrescriptionNumber Transmit(PharmaceuticalPrescriptionState prescription, string certficatePath, string certificatePassword);

        #endregion Methods

    }
}