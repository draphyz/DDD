using Conditions;
using System.Transactions;
using System.Threading.Tasks;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Domain.Prescriptions;
    using Core.Application;
    using Core.Domain;
    using Core.Infrastructure;

    public class PharmaceuticalPrescriptionRevoker : IAsyncCommandHandler<RevokePharmaceuticalPrescription>
    {

        #region Fields

        private readonly IDomainEventPublisher publisher;

        private readonly IAsyncRepository<PharmaceuticalPrescription> repository;

        #endregion Fields

        #region Constructors

        public PharmaceuticalPrescriptionRevoker(IAsyncRepository<PharmaceuticalPrescription> repository,
                                                 IDomainEventPublisher publisher)
        {
            Condition.Requires(repository, nameof(repository)).IsNotNull();
            Condition.Requires(publisher, nameof(publisher)).IsNotNull();
            this.repository = repository;
            this.publisher = publisher;
        }

        #endregion Constructors

        #region Methods

        public async Task HandleAsync(RevokePharmaceuticalPrescription command)
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            var prescription = await this.repository.FindAsync(new PrescriptionIdentifier(command.PrescriptionIdentifier));
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await this.RevokeAsync(prescription, command);
                this.publisher.PublishAll(prescription.AllEvents());
                scope.Complete();
            }
        }

        private async Task RevokeAsync(PharmaceuticalPrescription prescription,
                                       RevokePharmaceuticalPrescription command)
        {
            try
            {
                prescription.Revoke(command.RevocationReason);
                await this.repository.SaveAsync(prescription);
            }
            catch (RepositoryException ex)
            {
                throw new CommandException("The pharmaceutical prescription could not be revoked.", ex, command);
            }
        }

        #endregion Methods

    }
}
