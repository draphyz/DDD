using NHibernate;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Core.Infrastructure.Data;
    using Common.Domain;

    internal class Alpha2CountryCodeType : CompositeUserType<Alpha2CountryCode>
    {

        #region Constructors

        public Alpha2CountryCodeType()
        {
            this.Mutable(false);
            this.Property(c => c.Value, NHibernateUtil.AnsiString);
        }

        #endregion Constructors
    }
}
