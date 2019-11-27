using Conditions;
using System.Threading.Tasks;
using System.Transactions;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Application;
    using Core.Domain;
    using Domain.Prescriptions;

    public class PharmaceuticalPrescriptionRevoker
        : AsyncDomainCommandHandler<RevokePharmaceuticalPrescription>
    {

        #region Fields

        private readonly IEventPublisher publisher;
        private readonly IAsyncRepository<PharmaceuticalPrescription> repository;

        #endregion Fields

        #region Constructors

        public PharmaceuticalPrescriptionRevoker(IAsyncRepository<PharmaceuticalPrescription> repository,
                                                 IEventPublisher publisher)
        {
            Condition.Requires(repository, nameof(repository)).IsNotNull();
            Condition.Requires(publisher, nameof(publisher)).IsNotNull();
            this.repository = repository;
            this.publisher = publisher;
        }

        #endregion Constructors

        #region Methods

        protected override async Task ExecuteAsync(RevokePharmaceuticalPrescription command)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var prescription = await this.repository.FindAsync(new PrescriptionIdentifier(command.PrescriptionIdentifier));
                prescription.Revoke(command.RevocationReason);
                await this.repository.SaveAsync(prescription);
                this.publisher.PublishAll(prescription.AllEvents());
                scope.Complete();
            }
        }

        #endregion Methods

    }
}
