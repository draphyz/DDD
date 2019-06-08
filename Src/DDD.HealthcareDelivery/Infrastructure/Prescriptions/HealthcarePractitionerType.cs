using NHibernate;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Core.Infrastructure.Data;
    using Domain.Practitioners;

    public class HealthcarePractitionerType : CompositeUserType<HealthcarePractitioner>
    {
        public HealthcarePractitionerType()
        {
            this.Mutable(false);
            this.Property(p => p.Identifier);
            this.Discriminator("PrescriberType");
            this.Property(p => p.FullName, NHibernateUtil.Custom(typeof(FullNameType)));
            this.Property(p => p.DisplayName, NHibernateUtil.AnsiString);
            this.Property(p => p.LicenseNumber, NHibernateUtil.Custom(typeof(HealthcarePractitionerLicenseNumberType<BelgianHealthcarePractitionerLicenseNumber>)));
            this.Property(p => p.ContactInformation, NHibernateUtil.Custom(typeof(ContactInformationType)));
            this.Subcomponent<Physician>();
        }


        //        this.Property(p => p.Prescriber.PractitionerType)
        //            .HasColumnName(ToCasingConvention("PrescriberType"))
        //            .HasColumnOrder(8)
        //            .IsUnicode(false)
        //            .HasMaxLength(20)
        //            .IsRequired();



        //        this.Property(p => p.Prescriber.SocialSecurityNumber)
        //            .HasColumnName(ToCasingConvention("PrescriberSSN"))
        //            .HasColumnOrder(13)
        //            .IsUnicode(false)
        //            .HasMaxLength(25);
        //        this.Property(p => p.Prescriber.Speciality)
        //            .HasColumnName(ToCasingConvention("PrescriberSpeciality"))
        //            .HasColumnOrder(14)
        //            .IsUnicode(false)
        //            .HasMaxLength(50);
    }
}
