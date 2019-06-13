using Conditions;
using System;

namespace DDD.Common.Domain
{
    /// <summary>
    /// Represents the Belgian social security number (NISS).
    /// </summary>
    public class BelgianSocialSecurityNumber : SocialSecurityNumber
    {

        #region Constructors

        public BelgianSocialSecurityNumber(string value) : base(value)
        {
            Condition.Requires(value, nameof(value))
                     .HasLength(11)
                     .Evaluate(c => c.IsNumeric());
        }

        #endregion Constructors

        #region Enums

        public enum Sex
        {
            Male,
            Female
        }

        #endregion Enums

        #region Methods

        /// <summary>
        /// Computes the check digit based on the 9 first digits.
        /// </summary>
        public static int ComputeCheckDigit(string value, bool bornBefore2000 = true)
        {
            Condition.Requires(value, nameof(value)).IsLongerOrEqual(9);
            long identifier;
            if (bornBefore2000)
                identifier = long.Parse(value.Substring(0, 9));
            else
                identifier = long.Parse($"2{value.Substring(0, 9)}");
            var modulus = 97;
            var remainder = modulus - (identifier % modulus);
            if (remainder == 0) return modulus;
            return (int)remainder;
        }

        public static BelgianSocialSecurityNumber CreateIfNotEmpty(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return new BelgianSocialSecurityNumber(value);
        }

        /// <summary>
        /// Returns the birthdate of the person, if known.
        /// </summary>
        public DateTime? Birthdate()
        {
            var birthMonth = this.BirthMonth();
            if (birthMonth == null) return null;
            return new DateTime(this.BirthYear().Value, birthMonth.Value, this.BirthDay().Value);
        }

        /// <summary>
        /// Returns the day of birth of the person, if known.
        /// </summary>
        public int? BirthDay()
        {
            if (this.BirthMonth() == null) return null;
            return int.Parse(this.Value.Substring(4, 2));
        }

        /// <summary>
        /// Returns the month of birth of the person, if known.
        /// </summary>
        public int? BirthMonth()
        {
            var month = this.MonthNumber();
            if (month >= 40)
                month -= 40;
            else if (month >= 20)
                month -= 20;
            if (month == 0) return null;
            return month;
        }

        /// <summary>
        /// Returns the year of birth of the person, if known.
        /// </summary>
        public int? BirthYear()
        {
            var year = this.Value.Substring(0, 2);
            if (year == "00") return null;
            if (this.BornBefore2000())
                return int.Parse($"19{year}");
            return int.Parse($"20{year}");
        }
        /// <summary>
        /// Determines whether the person was born before 1st January 2000.
        /// </summary>
        public bool BornBefore2000()
        {
            var checkDigit = this.CheckDigit();
            var computedCheckDigit = ComputeCheckDigit(this.Value, bornBefore2000: true);
            if (computedCheckDigit == checkDigit) return true;
            computedCheckDigit = ComputeCheckDigit(this.Value, bornBefore2000: false);
            if (computedCheckDigit == checkDigit) return false;
            throw new InvalidOperationException("The check digit of the number is invalid.");
        }

        /// <summary>
        /// Returns the check digit based on the 9 first digits.
        /// </summary>
        public int CheckDigit() => int.Parse(this.Value.Substring(9, 2));

        /// <summary>
        /// Determines whether the birthdate is only partially known.
        /// </summary>
        public bool HasPartialBirthdate() => this.BirthMonth() == null && this.BirthYear() != null;

        /// <summary>
        /// Determines whether the birthdate is unknown.
        /// </summary>
        public bool HasUnknownBirthdate() => this.BirthYear() == null;

        /// <summary>
        /// Determines whether the social security number is a BIS Register Number (for foreigners).
        /// </summary>
        public bool IsBisRegisterNumber() => !this.IsNationalRegisterNumber();

        /// <summary>
        /// Determines whether the social security number is a National Register Number.
        /// </summary>
        public bool IsNationalRegisterNumber() => this.MonthNumber() <= 12;

        /// <summary>
        /// Returns the unique identifier of the person.
        /// </summary>
        public int PersonUniqueIdentifier() => int.Parse(this.Value.Substring(0, 9));

        /// <summary>
        /// Returns a sequence number used for distinguishing people with identical birthdates.
        /// </summary>
        /// <remarks>
        /// This number is odd for males and even for females.
        /// </remarks>
        public int SequenceNumber()
        {
            if (this.HasPartialBirthdate()) return int.Parse(this.Value.Substring(5, 4));
            return int.Parse(this.Value.Substring(6, 3));
        }

        /// <summary>
        /// Returns the sex of the person assigned at birth.
        /// </summary>
        public Sex SexAtBirth() => this.SequenceNumber() % 2 == 0 ? Sex.Female : Sex.Male;

        private int MonthNumber() => int.Parse(this.Value.Substring(2, 2));

        #endregion Methods

    }
}
