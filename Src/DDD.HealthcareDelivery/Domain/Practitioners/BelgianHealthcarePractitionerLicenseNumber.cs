using Conditions;

namespace DDD.HealthcareDelivery.Domain.Practitioners
{
    /// <summary>
    /// Represents a license number attributed to Belgian healthcare practitioners by the Belgian National Institute for Health and Disability Insurance (INAMI/RIZIV).
    /// </summary>
    public class BelgianHealthcarePractitionerLicenseNumber : HealthcarePractitionerLicenseNumber
    {

        #region Constructors

        public BelgianHealthcarePractitionerLicenseNumber(string number) : base(number)
        {
            Condition.Requires(number, nameof(number))
                     .HasLength(11)
                     .Evaluate(n => n.IsNumeric());
        }

        #endregion Constructors

        #region Enums

        public enum BelgianProvince
        {
            FlemishBrabant = 0,
            Antwerp = 1,
            WalloonBrabant = 2,
            WestFlanders = 3,
            EastFlanders = 4,
            Hainaut = 5,
            Liege = 6,
            Limburg = 7,
            Luxembourg = 8,
            Namur = 9
        }

        public enum HealthProfession
        {
            Physician = 1,
            Pharmacist = 2,
            Dentist = 3,
            NurseOrMidwife = 4,
            Physiotherapist = 5,
            Paramedic = 6
        }

        public enum Modulus
        {
            Mod79 = 79,
            Mod83 = 83,
            Mod89 = 89,
            Mod97 = 97
        }

        #endregion Enums

        #region Methods

        /// <summary>
        /// Computes the check digit based on the 6 first digits.
        /// </summary>
        public static int ComputeCheckDigit(string number, Modulus modulus = Modulus.Mod97)
        {
            Condition.Requires(number, nameof(number)).IsLongerOrEqual(6);
            var identifier = int.Parse(number.Substring(0, 6)); // old unique practitioner identifier
            var imodulus = (int)modulus;
            return imodulus - (identifier % imodulus);
        }

        /// <summary>
        /// Returns the check digit based on the 6 first digits.
        /// </summary>
        public int CheckDigit() => int.Parse(this.Number.Substring(6, 2));

        /// <summary>
        /// Returns the registration number attributed to the healthcare practitioner by the Provincial Medical Commission.
        /// </summary>
        public int PractitionerIdentifier() => int.Parse(this.Number.Substring(2, 4));

        /// <summary>
        /// Returns the unique identifier of the healthcare practitioner.
        /// </summary>
        public int PractitionerUniqueIdentifier() => int.Parse(this.Number.Substring(0, 8));

        /// <summary>
        /// Returns the profession of the healthcare practitioner.
        /// </summary>
        public HealthProfession Profession() => (HealthProfession)int.Parse(this.Number[0].ToString());

        /// <summary>
        /// Returns the province of the healthcare practitioner at the registration time.
        /// </summary>
        public BelgianProvince Province() => (BelgianProvince)int.Parse(this.Number[1].ToString());

        /// <summary>
        /// Returns the professional qualification code of the healthcare practitioner.
        /// </summary>
        public string QualificationCode() => this.Number.Substring(8, 3);

        #endregion Methods

    }
}
