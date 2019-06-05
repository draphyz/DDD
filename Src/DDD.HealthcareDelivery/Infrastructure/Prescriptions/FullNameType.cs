using NHibernate;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Common.Domain;
    using Core.Infrastructure.Data;

    public class FullNameType : CompositeUserType<FullName>
    {

        #region Constructors

        public FullNameType()
        {
            this.Mutable(false);
            this.Property(n => n.LastName, NHibernateUtil.AnsiString);
            this.Property(n => n.FirstName, NHibernateUtil.AnsiString);
        }

        #endregion Constructors

    }
}