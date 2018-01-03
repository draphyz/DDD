namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Common.Domain;

    public class PrescriptionStatus : Enumeration
    {
        #region Fields

        public static readonly PrescriptionStatus

            Created = new PrescriptionStatus(1, "CRT", "Created"),
            Revoked = new PrescriptionStatus(2, "RVK", "Revoked"),
            Sent = new PrescriptionStatus(3, "SNT", "Sent"),
            Delivered = new PrescriptionStatus(4, "DLV", "Delivered");

        #endregion Fields

        #region Constructors

        private PrescriptionStatus(int value, string code, string name) : base(value, code, name)
        {
        }

        #endregion Constructors
    }
}