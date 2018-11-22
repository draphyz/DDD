using Conditions;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Common.Domain;

    public class PrescriptionIdentifier : ArbitraryIdentifier<int>
    {

        #region Constructors

        public PrescriptionIdentifier(int identifier) : base(identifier)
        {
            Condition.Requires(identifier, nameof(identifier)).IsGreaterThan(0);
        }

        #endregion Constructors

    }
}