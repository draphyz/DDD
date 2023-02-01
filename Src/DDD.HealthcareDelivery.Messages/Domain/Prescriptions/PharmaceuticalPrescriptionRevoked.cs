using EnsureThat;
using System;
using System.Runtime.Serialization;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Domain;

    [DataContract(Namespace = "http://schemas.ddd.com/healthcare-delivery")]
    public class PharmaceuticalPrescriptionRevoked : IDomainEvent
    {

        #region Constructors

        public PharmaceuticalPrescriptionRevoked(int prescriptionIdentifier, DateTime occurredOn, string reason = null)
        {
            Ensure.That(prescriptionIdentifier, nameof(prescriptionIdentifier)).IsGt(0);
            Ensure.That(reason, nameof(reason)).IsNotNullOrWhiteSpace();
            this.PrescriptionIdentifier = prescriptionIdentifier;
            this.Reason = reason;
            this.OccurredOn = occurredOn;
        }

        /// <remarks>For serialization</remarks>
        private PharmaceuticalPrescriptionRevoked() { }

        #endregion Constructors

        #region Properties

        [DataMember(Name = "PrescriptionId")]
        public int PrescriptionIdentifier { get; private set; }

        [DataMember(Name = "OccurredOn")]
        public DateTime OccurredOn { get; private set; }

        [DataMember(Name = "Reason")]
        public string Reason { get; private set; }

        #endregion Properties

    }
}
