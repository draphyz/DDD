using System.Transactions;
using System.Threading.Tasks;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Domain.Prescriptions;
    using Core.Application;
    using Core.Domain;

    public class PharmaceuticalPrescriptionRevoker 
        : AsyncRepositoryCommandHandler<RevokePharmaceuticalPrescription, PharmaceuticalPrescription>
    {

        #region Constructors

        public PharmaceuticalPrescriptionRevoker(IAsyncRepository<PharmaceuticalPrescription> repository,
                                                 IEventPublisher publisher)
            :base(repository, publisher)
        {
        }

        #endregion Constructors

        #region Methods

        protected override async Task ExecuteAsync(RevokePharmaceuticalPrescription command)
        {
            var prescription = await this.Repository.FindAsync(new PrescriptionIdentifier(command.PrescriptionIdentifier));
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                prescription.Revoke(command.RevocationReason);
                await this.Repository.SaveAsync(prescription);
                this.Publisher.PublishAll(prescription.AllEvents());
                scope.Complete();
            }
        }

        #endregion Methods

    }
}
