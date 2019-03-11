using Conditions;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    /// <summary>
    /// Represents the Belgian national medication code (CNK) attributed by the Belgian Association of Pharmacists (APB). 
    /// </summary>
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

        /// <summary>
        /// Computes the check digit based on the 6 first digits.
        /// </summary>
        public static int ComputeCheckDigit(string code)
        {
            Condition.Requires(code, nameof(code))
                     .IsLongerOrEqual(6)
                     .Evaluate(c => c.IsNumeric());
            var identifier = code.Substring(0, 6);
            var sum = 0;
            var alternate = true;
            for (int i = identifier.Length - 1; i >= 0; i--)
            {
                var n = int.Parse(identifier[i].ToString());
                if (alternate)
                {
                    n *= 2;
                    if (n > 9)
                    {
                        n = (n % 10) + 1;
                    }
                }
                sum += n;
                alternate = !alternate;
            }
            var remainder = sum % 10;
            if (remainder == 0) return remainder;
            return 10 - remainder;
        }

        public static BelgianMedicationCode CreateIfNotEmpty(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return null;
            return new BelgianMedicationCode(code);
        }

        /// <summary>
        /// Returns the check digit based on the 6 first digits.
        /// </summary>
        public int CheckDigit() => int.Parse(this.Code[6].ToString());

        /// <summary>
        /// Returns the unique identifier of the medication.
        /// </summary>
        public int MedicationUniqueIdentifier() => int.Parse(this.Code.Substring(0, 6));

        #endregion Methods

    }
}
