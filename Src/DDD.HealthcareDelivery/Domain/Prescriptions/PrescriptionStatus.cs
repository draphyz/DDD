namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Common.Domain;

    public class PrescriptionStatus : Enumeration
    {

        #region Fields

        public static readonly PrescriptionStatus

            Created = new PrescriptionStatus(1, "CRT", "Created"),
            InProcess = new PrescriptionStatus(2, "INP", "InProcess"),
            Delivered = new PrescriptionStatus(3, "DLV", "Delivered"),
            Revoked = new PrescriptionStatus(4, "RVK", "Revoked"),
            Expired = new PrescriptionStatus(5, "EXP", "Expired"),
            Archived = new PrescriptionStatus(6, "ARC", "Archived");

        #endregion Fields

        #region Constructors

        private PrescriptionStatus(int value, string code, string name) : base(value, code, name)
        {
        }

        #endregion Constructors

    }
}