using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain.Prescriptions;

    internal abstract class PrescriptionStateConfiguration : IEntityTypeConfiguration<PrescriptionState>
    {

        #region Methods

        /// <remarks>
        /// IsRequired() on properties of owned entities is ignored.
        /// According to EF Core team, "value objects would work better if implemented using value converters".
        /// https://github.com/dotnet/efcore/issues/18445
        /// https://github.com/dotnet/efcore/issues/16943
        /// </remarks>
        public virtual void Configure(EntityTypeBuilder<PrescriptionState> builder)
        {
            // Table
            builder.ToTable("Prescription");
            // Keys
            builder.HasKey(p => p.Identifier);
            // Discriminator
            builder.HasDiscriminator<string>("PrescriptionType")
                   .HasValue<PharmaceuticalPrescriptionState>("PHARM");
            // Fields
            builder.Property<string>("PrescriptionType")
                   .HasMaxLength(5)
                   .IsRequired();
            builder.Property(p => p.Identifier)
                   .HasColumnName("PrescriptionId")
                   .ValueGeneratedNever();
            builder.Property(p => p.Status)
                   .HasMaxLength(3)
                   .IsFixedLength()
                   .IsRequired();
            builder.Property(p => p.LanguageCode)
                   .HasColumnName("Language")
                   .HasMaxLength(2)
                   .IsFixedLength()
                   .IsRequired();
            builder.Property(p => p.DeliverableAt)
                   .HasColumnType("date");
            builder.OwnsOne(p => p.Prescriber, prescriber =>
            {
                prescriber.Property(p => p.Identifier)
                          .HasColumnName("PrescriberId");
                prescriber.Property(p => p.PractitionerType)
                          .HasColumnName("PrescriberType")
                          .HasMaxLength(20);
                prescriber.OwnsOne(p => p.FullName, fullName =>
                {
                    fullName.Property(n => n.LastName)
                            .HasColumnName("PrescriberLastName")
                            .HasMaxLength(50);
                    fullName.Property(n => n.FirstName)
                            .HasColumnName("PrescriberFirstName")
                            .HasMaxLength(50);
                });
                prescriber.Property(p => p.DisplayName)
                          .HasColumnName("PrescriberDisplayName")
                          .HasMaxLength(100);
                prescriber.Property(p => p.LicenseNumber)
                          .HasColumnName("PrescriberLicenseNum")
                          .HasMaxLength(25);
                prescriber.Property(p => p.SocialSecurityNumber)
                          .HasColumnName("PrescriberSSN")
                          .HasMaxLength(25);
                prescriber.Property(p => p.Speciality)
                          .HasColumnName("PrescriberSpeciality")
                          .HasMaxLength(50);
                prescriber.OwnsOne(p => p.ContactInformation, contactInfo =>
                {
                    contactInfo.Property(i => i.PrimaryTelephoneNumber)
                               .HasColumnName("PrescriberPhone1")
                               .HasMaxLength(20);
                    contactInfo.Property(i => i.SecondaryTelephoneNumber)
                               .HasColumnName("PrescriberPhone2")
                               .HasMaxLength(20);
                    contactInfo.Property(i => i.PrimaryEmailAddress)
                               .HasColumnName("PrescriberEmail1")
                               .HasMaxLength(50);
                    contactInfo.Property(i => i.SecondaryEmailAddress)
                               .HasColumnName("PrescriberEmail2")
                               .HasMaxLength(50);
                    contactInfo.Property(i => i.WebSite)
                               .HasColumnName("PrescriberWebSite")
                               .HasMaxLength(255);
                    contactInfo.OwnsOne(i => i.PostalAddress, address =>
                    {
                        address.Property(a => a.Street)
                               .HasColumnName("PrescriberStreet")
                               .HasMaxLength(50);
                        address.Property(a => a.HouseNumber)
                               .HasColumnName("PrescriberHouseNum")
                               .HasMaxLength(10);
                        address.Property(a => a.BoxNumber)
                               .HasColumnName("PrescriberBoxNum")
                               .HasMaxLength(10);
                        address.Property(a => a.PostalCode)
                               .HasColumnName("PrescriberPostCode")
                               .HasMaxLength(10);
                        address.Property(a => a.City)
                               .HasColumnName("PrescriberCity")
                               .HasMaxLength(50);
                        address.Property(a => a.CountryCode)
                               .HasColumnName("PrescriberCountry")
                               .HasMaxLength(2)
                               .IsFixedLength();
                    });
                    contactInfo.Ignore(i => i.FaxNumber);
                });
            });
            builder.OwnsOne(p => p.Patient, patient =>
            {
                patient.Property(p => p.Identifier)
                       .HasColumnName("PatientId");
                patient.OwnsOne(p => p.FullName, fullName =>
                {
                    fullName.Property(n => n.FirstName)
                            .HasColumnName("PatientFirstName")
                            .HasMaxLength(50);
                    fullName.Property(n => n.LastName)
                            .HasColumnName("PatientLastName")
                            .HasMaxLength(50);
                });
                patient.Property(p => p.Sex)
                       .HasColumnName("PatientSex")
                       .HasMaxLength(2);
                patient.Property(p => p.SocialSecurityNumber)
                       .HasColumnName("PatientSSN")
                       .HasMaxLength(25);
                patient.Property(p => p.Birthdate)
                       .HasColumnName("PatientBirthdate")
                       .HasColumnType("date");
                patient.Ignore(p => p.ContactInformation);
            });
            builder.Property(p => p.EncounterIdentifier)
                   .HasColumnName("EncounterId");
        }

        #endregion Methods

    }
}
