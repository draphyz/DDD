using Conditions;
using System.Transactions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Mapping;
    using Core.Domain;
    using Domain.Prescriptions;
    using Core.Application;

    public class PharmaceuticalPrescriptionsCreator
        : RepositoryCommandHandler<CreatePharmaceuticalPrescriptions, PharmaceuticalPrescription>
    {

        #region Constructors

        public PharmaceuticalPrescriptionsCreator(IAsyncRepository<PharmaceuticalPrescription> repository,
                                                  IDomainEventPublisher publisher,
                                                  IObjectTranslator<CreatePharmaceuticalPrescriptions, IEnumerable<PharmaceuticalPrescription>> translator)
            : base(repository, publisher)
        {
            Condition.Requires(translator, nameof(translator)).IsNotNull();
            this.Translator = translator;
        }

        #endregion Constructors

        #region Properties

        protected IObjectTranslator<CreatePharmaceuticalPrescriptions, IEnumerable<PharmaceuticalPrescription>> Translator { get; }

        #endregion Properties

        #region Methods

        protected override async Task ExecuteAsync(CreatePharmaceuticalPrescriptions command)
        {
            var prescriptions = this.Translator.Translate(command);
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await this.Repository.SaveAllAsync(prescriptions);
                this.Publisher.PublishAll(prescriptions.AllEvents());
                scope.Complete();
            }
        }

        #endregion Methods

    }
}
