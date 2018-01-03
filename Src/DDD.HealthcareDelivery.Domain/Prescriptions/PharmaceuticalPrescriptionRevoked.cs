using Conditions;
using System;
using System.Runtime.Serialization;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Domain;

    [DataContract(Namespace = "DDD.HealthcareDelivery.Domain.Prescriptions")]
    public class PharmaceuticalPrescriptionRevoked : IDomainEvent
    {

        #region Constructors

        public PharmaceuticalPrescriptionRevoked(int prescriptionIdentifier, string reason, DateTime occuredOn)
        {
            Condition.Requires(prescriptionIdentifier, nameof(prescriptionIdentifier)).IsGreaterThan(0);
            Condition.Requires(reason, nameof(reason)).IsNotNullOrWhiteSpace();
            this.PrescriptionIdentifier = prescriptionIdentifier;
            this.Reason = reason;
            this.OccurredOn = occuredOn;
        }

        public PharmaceuticalPrescriptionRevoked(int prescriptionIdentifier, string reason)
            : this(prescriptionIdentifier, reason, DateTime.Now)
        {
        }

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
