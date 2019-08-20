using NHibernate;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Common.Domain;
    using Core.Infrastructure.Data;

    internal class ContactInformationType : CompositeUserType<ContactInformation>
    {

        #region Constructors

        public ContactInformationType()
        {
            this.Mutable(false);
            this.Property(c => c.PrimaryTelephoneNumber, NHibernateUtil.AnsiString);
            this.Property(c => c.SecondaryTelephoneNumber, NHibernateUtil.AnsiString);
            this.Property(c => c.PrimaryEmailAddress, NHibernateUtil.Custom(typeof(EmailAddressType)));
            this.Property(c => c.SecondaryEmailAddress, NHibernateUtil.Custom(typeof(EmailAddressType)));
            this.Property(c => c.WebSite, new AnsiUriType());
            this.Property(c => c.PostalAddress, NHibernateUtil.Custom(typeof(PostalAddressType)));
        }

        #endregion Constructors

    }
}
