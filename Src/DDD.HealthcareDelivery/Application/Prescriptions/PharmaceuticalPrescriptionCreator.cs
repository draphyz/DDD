using Conditions;
using System.Transactions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Mapping;
    using Core.Domain;
    using Domain.Prescriptions;
    using Core.Application;

    public class PharmaceuticalPrescriptionCreator
        : AsyncRepositoryCommandHandler<CreatePharmaceuticalPrescription, PharmaceuticalPrescription>
    {

        #region Constructors

        public PharmaceuticalPrescriptionCreator(IAsyncRepository<PharmaceuticalPrescription> repository,
                                                 IEventPublisher publisher,
                                                 IObjectTranslator<CreatePharmaceuticalPrescription, PharmaceuticalPrescription> translator)
            : base(repository, publisher)
        {
            Condition.Requires(translator, nameof(translator)).IsNotNull();
            this.Translator = translator;
        }

        #endregion Constructors

        #region Properties

        protected IObjectTranslator<CreatePharmaceuticalPrescription, PharmaceuticalPrescription> Translator { get; }

        #endregion Properties

        #region Methods

        protected override async Task ExecuteAsync(CreatePharmaceuticalPrescription command)
        {
            var prescription = this.Translator.Translate(command);
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await this.Repository.SaveAsync(prescription);
                this.Publisher.PublishAll(prescription.AllEvents());
                scope.Complete();
            }
        }

        #endregion Methods

    }
}
