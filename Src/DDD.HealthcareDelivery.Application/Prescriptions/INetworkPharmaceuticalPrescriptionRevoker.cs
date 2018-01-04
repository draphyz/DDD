namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Domain.Prescriptions;

    public interface INetworkPharmaceuticalPrescriptionRevoker
    {

        #region Methods

        void Revoke(PharmaceuticalPrescriptionState prescription, string reason);

        #endregion Methods

    }
}