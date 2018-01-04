using Conditions;
using System.Transactions;
using System;

namespace Xperthis.HealthcareDelivery.Application.Prescriptions
{
    using Domain.Prescriptions;
    using Core.Application;
    using Core.Domain;
    using Core.Infrastructure;

    public class PharmaceuticalPrescriptionRevoker : ICommandHandler<RevokePharmaceuticalPrescription>
    {

        #region Fields

        private readonly IDomainEventPublisher publisher;

        private readonly IRepository<PharmaceuticalPrescription> repository;

        private readonly INetworkPharmaceuticalPrescriptionRevoker networkRevoker;

        #endregion Fields

        #region Constructors

        public PharmaceuticalPrescriptionRevoker(IRepository<PharmaceuticalPrescription> repository,
                                                 INetworkPharmaceuticalPrescriptionRevoker networkRevoker,
                                                 IDomainEventPublisher publisher)
        {
            Condition.Requires(repository, nameof(repository)).IsNotNull();
            Condition.Requires(networkRevoker, nameof(networkRevoker)).IsNotNull();
            Condition.Requires(publisher, nameof(publisher)).IsNotNull();
            this.repository = repository;
            this.networkRevoker = networkRevoker;
            this.publisher = publisher;
        }

        #endregion Constructors

        #region Methods

        public void Handle(RevokePharmaceuticalPrescription command)
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            var prescription = this.repository.Find(new PrescriptionIdentifier(command.PrescriptionIdentifier));
            using (var scope = new TransactionScope())
            {
                this.Revoke(prescription, command);
                this.publisher.PublishAll(prescription.AllEvents());
                scope.Complete();
            }
        }

        private void Revoke(PharmaceuticalPrescription prescription,
                            RevokePharmaceuticalPrescription command)
        {
            try
            {
                if (prescription is ElectronicPharmaceuticalPrescription)
                    this.networkRevoker.Revoke(prescription.ToState(),
                                               command.RevocationReason,
                                               command.PrescriberCertificatePath,
                                               command.PrescriberCertificatePassword);
                prescription.Revoke(command.RevocationReason);
                this.repository.Save(prescription);
            }
            catch (Exception ex) when (ex is RepositoryException || ex is ClientServerRequestException)
            {
                throw new CommandException("The pharmaceutical prescription could not be revoked.", ex, command);
            }
        }

        #endregion Methods

    }
}
