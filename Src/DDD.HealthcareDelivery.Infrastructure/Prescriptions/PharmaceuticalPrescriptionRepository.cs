using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Core.Mapping;
    using Core.Domain;
    using Core.Infrastructure.Serialization;
    using Domain.Prescriptions;
    using Core.Infrastructure.Data;

    public class PharmaceuticalPrescriptionRepository
        : EFRepositoryWithEventStoring<PharmaceuticalPrescription, PharmaceuticalPrescriptionState, HealthcareContext>,
          IRepository<PharmaceuticalPrescription>
    {

        #region Constructors

        public PharmaceuticalPrescriptionRepository(IObjectTranslator<PharmaceuticalPrescriptionState, PharmaceuticalPrescription> prescriptionTranslator,
                                                    IObjectTranslator<IDomainEvent, StoredEvent> eventTranslator,
                                                    IDbContextFactory<HealthcareContext> contextFactory)
            : base(prescriptionTranslator, eventTranslator, contextFactory)
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
