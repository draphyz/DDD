using NHibernate;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Core.Infrastructure.Data;
    using Common.Domain;

    internal class EmailAddressType : CompositeUserType<EmailAddress>
    {

        #region Constructors

        public EmailAddressType()
        {
            this.Mutable(false);
            this.Property(a => a.Value, NHibernateUtil.AnsiString);
        }

        #endregion Constructors
    }
}
