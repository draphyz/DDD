namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Mapping;
    using Core.Domain;
    using Core.Application;
    using Domain.Prescriptions;
    using Core.Infrastructure.Data;
    using Domain;

    public class PharmaceuticalPrescriptionRepository
        : NHRepository<HealthcareDeliveryContext, PharmaceuticalPrescription, PrescriptionIdentifier>
    {
        #region Constructors

        public PharmaceuticalPrescriptionRepository(ISessionFactory<HealthcareDeliveryContext> sessionFactory, 
                                                    IObjectTranslator<IEvent, Event> eventTranslator) 
            : base(sessionFactory, eventTranslator)
        {
        }

        #endregion Constructors
    }
}
