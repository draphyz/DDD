namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Common.Domain;

    internal class BelgianSqlServerPrescriptionMapping
        : SqlServerPrescriptionMapping<BelgianSocialSecurityNumber, BelgianSex>
    {
    }
}