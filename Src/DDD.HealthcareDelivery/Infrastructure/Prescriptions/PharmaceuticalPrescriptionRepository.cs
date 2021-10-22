using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Mapping;
    using Core.Domain;
    using Domain.Prescriptions;
    using Core.Infrastructure.Data;

    public class PharmaceuticalPrescriptionRepository
        : EFRepository<PharmaceuticalPrescription, PharmaceuticalPrescriptionState, PrescriptionIdentifier>
    {

        #region Constructors

        public PharmaceuticalPrescriptionRepository(HealthcareDeliveryContext context,
                                                    IObjectTranslator<PharmaceuticalPrescriptionState, PharmaceuticalPrescription> prescriptionTranslator,
                                                    IObjectTranslator<IEvent, StoredEvent> eventTranslator)
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
