using Conditions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Application;
    using Core.Domain;
    using Domain.Prescriptions;
    using Mapping;

    public class PharmaceuticalPrescriptionCreator
        : AsyncDomainCommandHandler<CreatePharmaceuticalPrescription>
    {

        #region Fields

        private readonly IAsyncRepository<PharmaceuticalPrescription, PrescriptionIdentifier> repository;
        private readonly IObjectTranslator<CreatePharmaceuticalPrescription, PharmaceuticalPrescription> translator;

        #endregion Fields

        #region Constructors

        public PharmaceuticalPrescriptionCreator(IAsyncRepository<PharmaceuticalPrescription, PrescriptionIdentifier> repository,
                                                 IObjectTranslator<CreatePharmaceuticalPrescription, PharmaceuticalPrescription> translator)
        {
            Condition.Requires(repository, nameof(repository)).IsNotNull();
            Condition.Requires(translator, nameof(translator)).IsNotNull();
            this.repository = repository;
            this.translator = translator;
        }

        #endregion Constructors

        #region Methods

        protected override async Task ExecuteAsync(CreatePharmaceuticalPrescription command, CancellationToken cancellationToken = default)
        {
            var prescription = this.translator.Translate(command);
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await this.repository.SaveAsync(prescription, cancellationToken);
                scope.Complete();
            }
        }

        #endregion Methods

    }
}
