using Conditions;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Common.Domain;

    public class PrescriptionIdentifier : ArbitraryIdentifier<int>
    {

        #region Constructors

        public PrescriptionIdentifier(int value) : base(value)
        {
            Condition.Requires(value, nameof(value)).IsGreaterThan(0);
        }

        #endregion Constructors

    }
}