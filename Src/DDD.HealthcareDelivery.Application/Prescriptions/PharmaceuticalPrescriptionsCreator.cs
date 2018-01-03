using Conditions;
using System.Transactions;
using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Mapping;
    using Core.Domain;
    using Core.Infrastructure;
    using Domain.Prescriptions;
    using Core.Application;

    public class PharmaceuticalPrescriptionsCreator
        : ICommandHandler<CreatePharmaceuticalPrescriptions>
    {

        #region Fields

        private readonly IDomainEventPublisher publisher;

        private readonly IRepository<PharmaceuticalPrescription> repository;

        private readonly IObjectTranslator<CreatePharmaceuticalPrescriptions, IEnumerable<PharmaceuticalPrescription>> translator;

        #endregion Fields

        #region Constructors

        public PharmaceuticalPrescriptionsCreator(IObjectTranslator<CreatePharmaceuticalPrescriptions, IEnumerable<PharmaceuticalPrescription>> translator,
                                                  IRepository<PharmaceuticalPrescription> repository, 
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

        public void Handle(CreatePharmaceuticalPrescriptions command)
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            var prescriptions = this.translator.Translate(command);
            using (var scope = new TransactionScope())
            {
                this.Create(prescriptions, command);
                this.publisher.PublishAll(prescriptions.AllEvents());
                scope.Complete();
            }
        }

        private void Create(IEnumerable<PharmaceuticalPrescription> prescriptions, 
                            CreatePharmaceuticalPrescriptions command)
        {
            try
            {
                this.repository.SaveAll(prescriptions);
            }
            catch(RepositoryException ex)
            {
                throw new CommandException("The pharmaceutical prescriptions could not be created.", ex, command);
            }
        }

        #endregion Methods

    }
}
