using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain.Prescriptions;

    internal abstract class PrescriptionStateConfiguration : EntityTypeConfiguration<PrescriptionState>
    {

        #region Fields

        public const string Discriminator = "PrescriptionType";

        public const string TableName = "Prescription";

        private readonly bool useUpperCase;

        #endregion Fields

        #region Constructors

        protected PrescriptionStateConfiguration(bool useUpperCase)
        {
            this.useUpperCase = useUpperCase;
            // Keys
            this.HasKey(p => p.Identifier);
            // Fields
            this.Property(p => p.Identifier)
                .HasColumnName(ToCasingConvention("PrescriptionId"))
                .HasColumnOrder(1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None); ;
            this.Property(p => p.Status)
                .HasColumnOrder(3)
                .IsUnicode(false)
                .HasMaxLength(3)
                .IsFixedLength()
                .IsRequired();
            this.Property(p => p.LanguageCode)
                .HasColumnName(ToCasingConvention("Language"))
                .HasColumnOrder(4)
                .IsUnicode(false)
                .HasMaxLength(2)
                .IsFixedLength()
                .IsRequired();
            this.Property(p => p.CreatedOn)
                .HasColumnOrder(5);
            this.Property(p => p.DelivrableAt)
                .HasColumnOrder(6);
            this.Property(p => p.Prescriber.Identifier)
                .HasColumnName(ToCasingConvention("PrescriberId"))
                .HasColumnOrder(7);
            this.Property(p => p.Prescriber.ProviderType)
                .HasColumnName(ToCasingConvention("PrescriberType"))
                .HasColumnOrder(8)
                .IsUnicode(false)
                .HasMaxLength(20)
                .IsRequired();
            this.Property(p => p.Prescriber.FullName.LastName)
                .HasColumnName(ToCasingConvention("PrescriberLastName"))
                .HasColumnOrder(9)
                .IsUnicode(false)
                .HasMaxLength(50)
                .IsRequired();
            this.Property(p => p.Prescriber.FullName.FirstName)
                .HasColumnName(ToCasingConvention("PrescriberFirstName"))
                .HasColumnOrder(10)
                .IsUnicode(false)
                .HasMaxLength(50)
                .IsRequired();
            this.Property(p => p.Prescriber.DisplayName)
                .HasColumnName(ToCasingConvention("PrescriberDisplayName"))
                .HasColumnOrder(11)
                .IsUnicode(false)
                .HasMaxLength(100)
                .IsRequired();
            this.Property(p => p.Prescriber.LicenseNumber)
                .HasColumnName(ToCasingConvention("PrescriberLicenseNum"))
                .HasColumnOrder(12)
                .IsUnicode(false)
                .HasMaxLength(25)
                .IsRequired();
            this.Property(p => p.Prescriber.SocialSecurityNumber)
                .HasColumnName(ToCasingConvention("PrescriberSSN"))
                .HasColumnOrder(13)
                .IsUnicode(false)
                .HasMaxLength(25);
            this.Property(p => p.Prescriber.Speciality)
                .HasColumnName(ToCasingConvention("PrescriberSpeciality"))
                .HasColumnOrder(14)
                .IsUnicode(false)
                .HasMaxLength(50);
            this.Property(p => p.Prescriber.ContactInformation.PrimaryTelephoneNumber)
                .HasColumnName(ToCasingConvention("PrescriberPhone1"))
                .HasColumnOrder(15)
                .IsUnicode(false)
                .HasMaxLength(20);
            this.Property(p => p.Prescriber.ContactInformation.SecondaryTelephoneNumber)
                .HasColumnName(ToCasingConvention("PrescriberPhone2"))
                .HasColumnOrder(16)
                .IsUnicode(false)
                .HasMaxLength(20);
            this.Property(p => p.Prescriber.ContactInformation.PrimaryEmailAddress)
                .HasColumnName(ToCasingConvention("PrescriberEmail1"))
                .HasColumnOrder(17)
                .IsUnicode(false)
                .HasMaxLength(50);
            this.Property(p => p.Prescriber.ContactInformation.SecondaryEmailAddress)
                .HasColumnName(ToCasingConvention("PrescriberEmail2"))
                .HasColumnOrder(18)
                .IsUnicode(false)
                .HasMaxLength(50);
            this.Property(p => p.Prescriber.ContactInformation.WebSite)
                .HasColumnName(ToCasingConvention("PrescriberWebSite"))
                .HasColumnOrder(19)
                .IsUnicode(false)
                .HasMaxLength(255);
            this.Property(p => p.Prescriber.ContactInformation.PostalAddress.Street)
                .HasColumnName(ToCasingConvention("PrescriberStreet"))
                .HasColumnOrder(20)
                .IsUnicode(false)
                .HasMaxLength(50);
            this.Property(p => p.Prescriber.ContactInformation.PostalAddress.HouseNumber)
                .HasColumnName(ToCasingConvention("PrescriberHouseNum"))
                .HasColumnOrder(21)
                .IsUnicode(false)
                .HasMaxLength(10);
            this.Property(p => p.Prescriber.ContactInformation.PostalAddress.BoxNumber)
                .HasColumnName(ToCasingConvention("PrescriberBoxNum"))
                .HasColumnOrder(22)
                .IsUnicode(false)
                .HasMaxLength(10);
            this.Property(p => p.Prescriber.ContactInformation.PostalAddress.PostalCode)
                .HasColumnName(ToCasingConvention("PrescriberPostCode"))
                .HasColumnOrder(23)
                .IsUnicode(false)
                .HasMaxLength(10);
            this.Property(p => p.Prescriber.ContactInformation.PostalAddress.City)
                .HasColumnName(ToCasingConvention("PrescriberCity"))
                .HasColumnOrder(24)
                .IsUnicode(false)
                .HasMaxLength(50);
            this.Property(p => p.Prescriber.ContactInformation.PostalAddress.CountryCode)
                .HasColumnName(ToCasingConvention("PrescriberCountry"))
                .HasColumnOrder(25)
                .IsUnicode(false)
                .HasMaxLength(2)
                .IsFixedLength();
            this.Property(p => p.Patient.Identifier)
                .HasColumnName(ToCasingConvention("PatientId"))
                .HasColumnOrder(26);
            this.Property(p => p.Patient.FullName.FirstName)
                .HasColumnName(ToCasingConvention("PatientFirstName"))
                .HasColumnOrder(27)
                .IsUnicode(false)
                .HasMaxLength(50)
                .IsRequired();
            this.Property(p => p.Patient.FullName.LastName)
                .HasColumnName(ToCasingConvention("PatientLastName"))
                .HasColumnOrder(28)
                .IsUnicode(false)
                .HasMaxLength(50)
                .IsRequired();
            this.Property(p => p.Patient.Sex)
                .HasColumnName(ToCasingConvention("PatientSex"))
                .HasColumnOrder(29)
                .IsUnicode(false)
                .HasMaxLength(2)
                .IsRequired();
            this.Property(p => p.Patient.SocialSecurityNumber)
                .HasColumnName(ToCasingConvention("PatientSSN"))
                .HasColumnOrder(30)
                .IsUnicode(false)
                .HasMaxLength(25);
            this.Property(p => p.Patient.Birthdate)
                .HasColumnName(ToCasingConvention("PatientBirthdate"))
                .HasColumnOrder(31);
            this.Property(p => p.Patient.OldIdentifier)
                .HasColumnName(ToCasingConvention("PatientOldId"))
                .HasColumnOrder(32);
            this.Property(p => p.HealthFacility.Identifier)
                .HasColumnName(ToCasingConvention("FacilityId"))
                .HasColumnOrder(33);
            this.Property(p => p.HealthFacility.FacilityType)
                .HasColumnName(ToCasingConvention("FacilityType"))
                .HasColumnOrder(34)
                .IsUnicode(false)
                .HasMaxLength(20)
                .IsRequired();
            this.Property(p => p.HealthFacility.Name)
                .HasColumnName(ToCasingConvention("FacilityName"))
                .HasColumnOrder(35)
                .IsUnicode(false)
                .HasMaxLength(100)
                .IsRequired();
            this.Property(p => p.HealthFacility.LicenseNumber)
                .HasColumnName(ToCasingConvention("FacilityLicenseNum"))
                .HasColumnOrder(36)
                .IsUnicode(false)
                .HasMaxLength(25);
            this.Property(p => p.HealthFacility.Code)
                .HasColumnName(ToCasingConvention("FacilityCode"))
                .HasColumnOrder(37)
                .IsUnicode(false)
                .HasMaxLength(25);
        }

        #endregion Constructors

        #region Methods

        protected string ToCasingConvention(string name) => this.useUpperCase ? name.ToUpperInvariant() : name;

        #endregion Methods

    }
}
