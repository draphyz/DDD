using Conditions;

namespace DDD.HealthcareDelivery.Domain.Encounters
{
    using Common.Domain;

    public class EncounterIdentifier : ArbitraryIdentifier<int>
    {

        #region Constructors

        public EncounterIdentifier(int value) : base(value)
        {
            Condition.Requires(value, nameof(value)).IsGreaterThan(0);
        }

        #endregion Constructors

        #region Methods

        public static EncounterIdentifier CreateIfNotEmpty(int? value)
        {
            if (!value.HasValue) return null;
            return new EncounterIdentifier(value.Value);
        }

        #endregion Methods

    }
}