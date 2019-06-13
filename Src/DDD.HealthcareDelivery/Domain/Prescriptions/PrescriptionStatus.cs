namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Common.Domain;

    public class PrescriptionStatus : Enumeration
    {

        #region Fields

        public static readonly PrescriptionStatus

            Created = new PrescriptionStatus(1, "CRT", nameof(Created)),
            InProcess = new PrescriptionStatus(2, "INP", nameof(InProcess)),
            Delivered = new PrescriptionStatus(3, "DLV", nameof(Delivered)),
            Revoked = new PrescriptionStatus(4, "RVK", nameof(Revoked)),
            Expired = new PrescriptionStatus(5, "EXP", nameof(Expired)),
            Archived = new PrescriptionStatus(6, "ARC", nameof(Archived));

        #endregion Fields

        #region Constructors

        private PrescriptionStatus(int value, string code, string name) : base(value, code, name)
        {
        }

        #endregion Constructors

    }
}