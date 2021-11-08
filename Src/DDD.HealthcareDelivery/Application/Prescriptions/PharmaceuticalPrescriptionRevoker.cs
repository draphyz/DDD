using Conditions;
using System.Threading;
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

        private readonly IAsyncRepository<PharmaceuticalPrescription, PrescriptionIdentifier> repository;

        #endregion Fields

        #region Constructors

        public PharmaceuticalPrescriptionRevoker(IAsyncRepository<PharmaceuticalPrescription, PrescriptionIdentifier> repository)
        {
            Condition.Requires(repository, nameof(repository)).IsNotNull();
            this.repository = repository;
        }

        #endregion Constructors

        #region Methods

        protected override async Task ExecuteAsync(RevokePharmaceuticalPrescription command, CancellationToken cancellationToken = default)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var prescription = await this.repository.FindAsync(new PrescriptionIdentifier(command.PrescriptionIdentifier));
                prescription.Revoke(command.RevocationReason);
                await this.repository.SaveAsync(prescription, cancellationToken);
                scope.Complete();
            }
        }

        #endregion Methods

    }
}
