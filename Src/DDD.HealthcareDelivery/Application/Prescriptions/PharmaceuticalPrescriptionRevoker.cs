using EnsureThat;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Application;
    using Core.Domain;
    using Domain.Prescriptions;
    using Mapping;
    using Threading;
    

    public class PharmaceuticalPrescriptionRevoker
        : ICommandHandler<RevokePharmaceuticalPrescription>
    {

        #region Fields

        private readonly IRepository<PharmaceuticalPrescription, PrescriptionIdentifier> repository;
        private readonly CompositeTranslator<Exception, CommandException> exceptionTranslator;

        #endregion Fields

        #region Constructors

        public PharmaceuticalPrescriptionRevoker(IRepository<PharmaceuticalPrescription, PrescriptionIdentifier> repository)
        {
            Ensure.That(repository, nameof(repository)).IsNotNull();
            this.repository = repository;
            this.exceptionTranslator = new CompositeTranslator<Exception, CommandException>();
            this.exceptionTranslator.Register(new DomainToCommandExceptionTranslator());
            this.exceptionTranslator.RegisterFallback();
        }

        #endregion Constructors

        #region Methods

        public void Handle(RevokePharmaceuticalPrescription command, IMessageContext context = null)
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            try
            {
                using (var scope = new TransactionScope())
                {
                    var prescription = this.repository.Find(new PrescriptionIdentifier(command.PrescriptionIdentifier));
                    prescription.Revoke(command.RevocationReason);
                    this.repository.Save(prescription);
                    scope.Complete();
                }
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<CommandException>())
            {
                throw this.exceptionTranslator.Translate(ex, new { Command = command });
            }
        }

        public async Task HandleAsync(RevokePharmaceuticalPrescription command, IMessageContext context = null)
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            try
            {
                await new SynchronizationContextRemover();
                var cancellationToken = context?.CancellationToken() ?? default;
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var prescription = await this.repository.FindAsync(new PrescriptionIdentifier(command.PrescriptionIdentifier), cancellationToken);
                    prescription.Revoke(command.RevocationReason);
                    await this.repository.SaveAsync(prescription, cancellationToken);
                    scope.Complete();
                }
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<CommandException>())
            {
                throw this.exceptionTranslator.Translate(ex, new { Command = command });
            }
        }

        #endregion Methods

    }
}
