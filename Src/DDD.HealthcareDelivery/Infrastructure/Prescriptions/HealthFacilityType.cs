using NHibernate;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Core.Infrastructure.Data;
    using Domain.Facilities;

    internal class HealthFacilityType<TFacilityLicenseNumber> : CompositeUserType<HealthFacility>
        where TFacilityLicenseNumber : HealthFacilityLicenseNumber
    {
        #region Constructors

        public HealthFacilityType()
        {
            this.Mutable(false);
            this.Property(p => p.Identifier);
            this.Discriminator("FacilityType");
            this.Property(p => p.Name, NHibernateUtil.AnsiString);
            this.Property(p => p.LicenseNumber, NHibernateUtil.Custom(typeof(HealthFacilityLicenseNumberType<TFacilityLicenseNumber>)));
            this.Subclass<Hospital>();
            this.Subclass<MedicalOffice>();
        }

        #endregion Constructors

    }
}
