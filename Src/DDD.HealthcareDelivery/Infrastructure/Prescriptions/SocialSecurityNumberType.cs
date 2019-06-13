using NHibernate;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Core.Infrastructure.Data;
    using Common.Domain;

    internal class SocialSecurityNumberType<T> : CompositeUserType<T>
        where T : SocialSecurityNumber
    {

        #region Constructors

        public SocialSecurityNumberType()
        {
            this.Mutable(false);
            this.Property(n => n.Value, NHibernateUtil.AnsiString);
        }

        #endregion Constructors
    }
}
