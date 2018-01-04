using Conditions;
using System.Transactions;
using System;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Domain.Prescriptions;
    using Core.Application;
    using Core.Domain;
    using Core.Infrastructure;

    public class PharmaceuticalPrescriptionTransmitter : ICommandHandler<SendPharmaceuticalPrescription>
    {

        #region Fields

        private readonly IDomainEventPublisher publisher;

        private readonly IRepository<PharmaceuticalPrescription> repository;

        private readonly INetworkPharmaceuticalPrescriptionTransmitter networkTransmitter;

        #endregion Fields

        #region Constructors

        public PharmaceuticalPrescriptionTransmitter(IRepository<PharmaceuticalPrescription> repository,
                                                     INetworkPharmaceuticalPrescriptionTransmitter networkTransmitter,
                                                     IDomainEventPublisher publisher)
        {
            Condition.Requires(repository, nameof(repository)).IsNotNull();
            Condition.Requires(networkTransmitter, nameof(networkTransmitter)).IsNotNull();
            Condition.Requires(publisher, nameof(publisher)).IsNotNull();
            this.repository = repository;
            this.networkTransmitter = networkTransmitter;
            this.publisher = publisher;
        }

        #endregion Constructors

        #region Methods

        public void Handle(SendPharmaceuticalPrescription command)
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            var prescription = this.repository.Find(new PrescriptionIdentifier(command.PrescriptionIdentifier));
            var electronicPrescription = prescription as ElectronicPharmaceuticalPrescription;
            if (electronicPrescription == null)
                throw new ArgumentException($"The prescription identifier '{command.PrescriptionIdentifier}' is invalid : cannot send a handwritten prescription.", nameof(command));
            using (var scope = new TransactionScope())
            {
                this.Send(electronicPrescription, command);
                this.publisher.PublishAll(prescription.AllEvents());
                scope.Complete();
            }
        }

        private void Send(ElectronicPharmaceuticalPrescription prescription,
                          SendPharmaceuticalPrescription command)
        {
            try
            {
                var electronicNumber = this.networkTransmitter.Transmit(prescription.ToState());
                prescription.Send(electronicNumber);
                this.repository.Save(prescription);
            }
            catch (Exception ex) when (ex is RepositoryException || ex is ClientServerRequestException)
            {
                throw new CommandException("The pharmaceutical prescription could not be sent.", ex, command);
            }
        }

        #endregion Methods

    }
}
