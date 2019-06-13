namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Common.Domain;
    using Domain.Practitioners;
    using Domain.Facilities;

    internal class BelgianSqlServerPrescriptionMapping: SqlServerPrescriptionMapping<BelgianHealthcarePractitionerLicenseNumber,
                                                                                     BelgianHealthFacilityLicenseNumber,
                                                                                     BelgianSocialSecurityNumber, 
                                                                                     BelgianSex>
    {
    }
}