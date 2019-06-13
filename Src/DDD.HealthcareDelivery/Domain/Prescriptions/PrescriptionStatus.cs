namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Common.Domain;

    public class PrescriptionStatus : Enumeration
    {

        #region Fields

        public static readonly PrescriptionStatus

            Created = new PrescriptionStatus(0, "CRT", nameof(Created)),
            InProcess = new PrescriptionStatus(1, "INP", nameof(InProcess)),
            Delivered = new PrescriptionStatus(2, "DLV", nameof(Delivered)),
            Revoked = new PrescriptionStatus(3, "RVK", nameof(Revoked)),
            Expired = new PrescriptionStatus(4, "EXP", nameof(Expired)),
            Archived = new PrescriptionStatus(5, "ARC", nameof(Archived));

        #endregion Fields

        #region Constructors

        private PrescriptionStatus(int value, string code, string name) : base(value, code, name)
        {
        }

        #endregion Constructors

    }
}