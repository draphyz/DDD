namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Application;

    /// <summary>
    /// Encapsulates all information needed to send a pharmaceutical prescription to an electronic medical record system.
    /// </summary>
    public class SendPharmaceuticalPrescription : ICommand
    {

        #region Properties

        public string PrescriberCertificatePassword { get; set; }

        public string PrescriberCertificatePath { get; set; }

        public int PrescriptionIdentifier { get; set; }

        #endregion Properties

    }
}
