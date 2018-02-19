using Conditions;
using System.Transactions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Mapping;
    using Core.Domain;
    using Core.Infrastructure;
    using Domain.Prescriptions;
    using Core.Application;

    public class PharmaceuticalPrescriptionsCreator
        : IAsyncCommandHandler<CreatePharmaceuticalPrescriptions>
    {

        #region Fields

        private readonly IDomainEventPublisher publisher;

        private readonly IAsyncRepository<PharmaceuticalPrescription> repository;

        private readonly IObjectTranslator<CreatePharmaceuticalPrescriptions, IEnumerable<PharmaceuticalPrescription>> translator;

        #endregion Fields

        #region Constructors

        public PharmaceuticalPrescriptionsCreator(IObjectTranslator<CreatePharmaceuticalPrescriptions, IEnumerable<PharmaceuticalPrescription>> translator,
                                                  IAsyncRepository<PharmaceuticalPrescription> repository, 
                                                  IDomainEventPublisher publisher)
        {
            Condition.Requires(translator, nameof(translator)).IsNotNull();
            Condition.Requires(repository, nameof(repository)).IsNotNull();
            Condition.Requires(publisher, nameof(publisher)).IsNotNull();
            this.translator = translator;
            this.repository = repository;
            this.publisher = publisher;
        }

        #endregion Constructors

        #region Methods

        public async Task HandleAsync(CreatePharmaceuticalPrescriptions command)
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            var prescriptions = this.translator.Translate(command);
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await this.CreateAsync(prescriptions, command);
                this.publisher.PublishAll(prescriptions.AllEvents());
                scope.Complete();
            }
        }

        private async Task CreateAsync(IEnumerable<PharmaceuticalPrescription> prescriptions, 
                                       CreatePharmaceuticalPrescriptions command)
        {
            try
            {
                await this.repository.SaveAllAsync(prescriptions);
            }
            catch(RepositoryException ex)
            {
                throw new CommandException("The pharmaceutical prescriptions could not be created.", ex, command);
            }
        }

        #endregion Methods

    }
}
