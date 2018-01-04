namespace Xperthis.HealthcareDelivery.Application.Prescriptions
{
    using Domain.Prescriptions;

    public interface INetworkPharmaceuticalPrescriptionRevoker
    {

        #region Methods

        void Revoke(PharmaceuticalPrescriptionState prescription, string reason, string certficatePath, string certificatePassword);

        #endregion Methods

    }
}