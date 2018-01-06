using Conditions;
using System.Transactions;

namespace DDD.HealthcareDelivery.Application.Prescriptions
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

        #endregion Fields

        #region Constructors

        public PharmaceuticalPrescriptionRevoker(IRepository<PharmaceuticalPrescription> repository,
                                                 IDomainEventPublisher publisher)
        {
            Condition.Requires(repository, nameof(repository)).IsNotNull();
            Condition.Requires(publisher, nameof(publisher)).IsNotNull();
            this.repository = repository;
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
                prescription.Revoke(command.RevocationReason);
                this.repository.Save(prescription);
            }
            catch (RepositoryException ex)
            {
                throw new CommandException("The pharmaceutical prescription could not be revoked.", ex, command);
            }
        }

        #endregion Methods

    }
}
