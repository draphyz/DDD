using NHibernate;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Core.Infrastructure.Data;
    using Domain.Practitioners;

    public class HealthcarePractitionerLicenseNumberType<T> : CompositeUserType<T>
        where T : HealthcarePractitionerLicenseNumber
    {

        #region Constructors

        public HealthcarePractitionerLicenseNumberType()
        {
            this.Property(p => p.Value, NHibernateUtil.AnsiString);
        }

        #endregion Constructors
    }
}
