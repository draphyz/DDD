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

    public class PharmaceuticalPrescriptionRepository
        : EFRepository<DbHealthcareDeliveryContext, PharmaceuticalPrescription, PharmaceuticalPrescriptionState, PrescriptionIdentifier>
    {

        #region Constructors

        public PharmaceuticalPrescriptionRepository(IDbContextFactory<DbHealthcareDeliveryContext> contextFactory,
                                                    IObjectTranslator<PharmaceuticalPrescriptionState, PharmaceuticalPrescription> prescriptionTranslator,
                                                    IObjectTranslator<IEvent, Event> eventTranslator)
            : base(contextFactory, prescriptionTranslator, eventTranslator)
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
