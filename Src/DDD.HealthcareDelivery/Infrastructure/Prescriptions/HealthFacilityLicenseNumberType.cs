using NHibernate;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Core.Infrastructure.Data;
    using Domain.Facilities;

    internal class HealthFacilityLicenseNumberType<T> : CompositeUserType<T>
        where T : HealthFacilityLicenseNumber
    {

        #region Constructors

        public HealthFacilityLicenseNumberType()
        {
            this.Mutable(false);
            this.Property(p => p.Value, NHibernateUtil.AnsiString);
        }

        #endregion Constructors
    }
}
