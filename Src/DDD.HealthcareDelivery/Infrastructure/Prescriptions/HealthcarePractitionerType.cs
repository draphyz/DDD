using NHibernate;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Common.Domain;
    using Core.Infrastructure.Data;
    using Domain.Practitioners;

    internal class HealthcarePractitionerType<TPractitionerLicenseNumber, TSocialSecurityNumber> : CompositeUserType<HealthcarePractitioner>
        where TPractitionerLicenseNumber : HealthcarePractitionerLicenseNumber
        where TSocialSecurityNumber : SocialSecurityNumber
    {
        #region Constructors

        public HealthcarePractitionerType()
        {
            this.Mutable(false);
            this.Property(p => p.Identifier);
            this.Discriminator("PrescriberType");
            this.Property(p => p.FullName, NHibernateUtil.Custom(typeof(FullNameType)));
            this.Property(p => p.DisplayName, NHibernateUtil.AnsiString);
            this.Property(p => p.LicenseNumber, NHibernateUtil.Custom(typeof(HealthcarePractitionerLicenseNumberType<TPractitionerLicenseNumber>)));
            this.Property(p => p.SocialSecurityNumber, NHibernateUtil.Custom(typeof(SocialSecurityNumberType<TSocialSecurityNumber>)));
            this.Property(p => p.Speciality, NHibernateUtil.AnsiString);
            this.Property(p => p.ContactInformation, NHibernateUtil.Custom(typeof(ContactInformationType)));
            this.Subclass<Physician>();
        }

        #endregion Constructors

    }
}
