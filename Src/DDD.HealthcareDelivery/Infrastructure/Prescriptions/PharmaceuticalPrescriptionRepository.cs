using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Mapping;
    using Core.Domain;
    using Core.Application;
    using Domain.Prescriptions;
    using Core.Infrastructure.Data;
    using DDD.HealthcareDelivery.Domain;

    public class PharmaceuticalPrescriptionRepository
        : EFRepository<HealthcareDeliveryContext, PharmaceuticalPrescription, PharmaceuticalPrescriptionState, PrescriptionIdentifier>
    {

        #region Constructors

        public PharmaceuticalPrescriptionRepository(DbHealthcareDeliveryContext context,
                                                    IObjectTranslator<PharmaceuticalPrescriptionState, PharmaceuticalPrescription> prescriptionTranslator,
                                                    IObjectTranslator<IEvent, Event> eventTranslator)
            : base(context, prescriptionTranslator, eventTranslator)
        {
        }

        #endregion Constructors

        #region Methods

        protected override IEnumerable<Expression<Func<PharmaceuticalPrescriptionState, object>>> RelatedEntitiesPaths()
        {
            yield return p => p.PrescribedMedications;
        }

        #endregion Methods

    }
}
