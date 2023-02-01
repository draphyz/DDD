using EnsureThat;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Common.Domain;

    public class PrescriptionIdentifier : ArbitraryIdentifier<int>
    {

        #region Constructors

        public PrescriptionIdentifier(int value) : base(value)
        {
            Ensure.That(value, nameof(value)).IsGt(0);
        }

        protected PrescriptionIdentifier()
        {
        }

        #endregion Constructors

    }
}