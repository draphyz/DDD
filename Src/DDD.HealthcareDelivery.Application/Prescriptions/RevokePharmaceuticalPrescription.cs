namespace Xperthis.HealthcareDelivery.Application.Prescriptions
{
    using Core.Application;

    /// <summary>
    /// Encapsulates all information needed to revoke a pharmaceutical prescription.
    /// </summary>
    public class RevokePharmaceuticalPrescription : ICommand
    {

        #region Properties

        public string PrescriberCertificatePassword { get; set; }

        public string PrescriberCertificatePath { get; set; }

        public int PrescriptionIdentifier { get; set; }

        public string RevocationReason { get; set; }

        #endregion Properties

    }
}
