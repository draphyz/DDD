using Conditions;
using System;
using System.Runtime.Serialization;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Domain;

    [DataContract(Namespace = "DDD.HealthcareDelivery.Prescriptions")]
    public class PharmaceuticalPrescriptionCreated : IDomainEvent
    {

        #region Constructors

        public PharmaceuticalPrescriptionCreated(int prescriptionIdentifier, DateTime occurredOn)
        {
            Condition.Requires(prescriptionIdentifier, nameof(prescriptionIdentifier)).IsGreaterThan(0);
            this.PrescriptionIdentifier = prescriptionIdentifier;
            this.OccurredOn = occurredOn;
        }

        /// <remarks>For serialization</remarks>
        private PharmaceuticalPrescriptionCreated() { }

        #endregion Constructors

        #region Properties

        [DataMember(Name = "OccurredOn", Order = 2)]
        public DateTime OccurredOn { get; private set; }

        [DataMember(Name = "PrescriptionId", Order = 1)]
        public int PrescriptionIdentifier { get; private set; }

        #endregion Properties
    }
}
