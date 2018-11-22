using Conditions;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core;

    /// <remarks>Represents a Belgian medication code (CNK)</remarks>
    public class BelgianMedicationCode : MedicationCode
    {

        #region Constructors

        public BelgianMedicationCode(string code) : base(code)
        {
            Condition.Requires(code, nameof(code))
                     .HasLength(7)
                     .Evaluate(c => c.IsNumeric());
        }

        #endregion Constructors

        #region Methods

        public static BelgianMedicationCode CreateIfNotEmpty(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return null;
            return new BelgianMedicationCode(code);
        }

        #endregion Methods

    }
}
