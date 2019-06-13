using NHibernate;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Common.Domain;
    using Core.Infrastructure.Data;

    internal class PostalAddressType : CompositeUserType<PostalAddress>
    {
        #region Constructors

        public PostalAddressType()
        {
            this.Mutable(false);
            this.Property(a => a.Street, NHibernateUtil.AnsiString);
            this.Property(a => a.HouseNumber, NHibernateUtil.AnsiString);
            this.Property(a => a.BoxNumber, NHibernateUtil.AnsiString);
            this.Property(a => a.PostalCode, NHibernateUtil.AnsiString);
            this.Property(a => a.City, NHibernateUtil.AnsiString);
            this.Property(a => a.CountryCode, NHibernateUtil.Custom(typeof(Alpha2CountryCodeType)));
        }

        #endregion Constructors

    }
}
