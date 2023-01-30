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

    public class PharmaceuticalPrescriptionCreator : ICommandHandler<CreatePharmaceuticalPrescription>
    {

        #region Fields

        private readonly IRepository<PharmaceuticalPrescription, PrescriptionIdentifier> repository;
        private readonly IObjectTranslator<CreatePharmaceuticalPrescription, PharmaceuticalPrescription> commandTranslator;
        private readonly CompositeTranslator<Exception, CommandException> exceptionTranslator;

        #endregion Fields

        #region Constructors

        public PharmaceuticalPrescriptionCreator(IRepository<PharmaceuticalPrescription, PrescriptionIdentifier> repository,
                                                 IObjectTranslator<CreatePharmaceuticalPrescription, PharmaceuticalPrescription> commandTranslator)
        {
            Ensure.That(repository, nameof(repository)).IsNotNull();
            Ensure.That(commandTranslator, nameof(commandTranslator)).IsNotNull();
            this.repository = repository;
            this.commandTranslator = commandTranslator;
            this.exceptionTranslator = new CompositeTranslator<Exception, CommandException>();
            this.exceptionTranslator.Register(new DomainToCommandExceptionTranslator());
            this.exceptionTranslator.RegisterFallback();
        }

        #endregion Constructors

        #region Methods

        public void Handle(CreatePharmaceuticalPrescription command, IMessageContext context = null)
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            try
            {
                var prescription = this.commandTranslator.Translate(command);
                using (var scope = new TransactionScope())
                {
                    this.repository.Save(prescription);
                    scope.Complete();
                }
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<CommandException>())
            {
                throw this.exceptionTranslator.Translate(ex, new { Command = command });
            }
        }

        public async Task HandleAsync(CreatePharmaceuticalPrescription command, IMessageContext context = null)
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            try
            {
                await new SynchronizationContextRemover();
                var cancellationToken = context?.CancellationToken() ?? default;
                var prescription = this.commandTranslator.Translate(command);
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
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
