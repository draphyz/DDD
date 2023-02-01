namespace DDD.HealthcareDelivery.Domain
{
    using Core.Domain;

    public class HealthcareDeliveryContext : BoundedContext
    {
        #region Constructors

        public HealthcareDeliveryContext() : base("DLV", "HealthcareDelivery")
        {
        }

        #endregion Constructors
    }
}
