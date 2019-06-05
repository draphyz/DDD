using Conditions;
using System;
using System.Runtime.Serialization;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Domain;

    [DataContract(Namespace = "DDD.HealthcareDelivery.Domain.Prescriptions")]
    public class PharmaceuticalPrescriptionCreated : IDomainEvent
    {

        #region Constructors

        public PharmaceuticalPrescriptionCreated(int prescriptionIdentifier, DateTime occuredOn)
        {
            Condition.Requires(prescriptionIdentifier, nameof(prescriptionIdentifier)).IsGreaterThan(0);
            this.PrescriptionIdentifier = prescriptionIdentifier;
            this.OccurredOn = occuredOn;
        }

        /// <remarks>For serialization</remarks>
        private PharmaceuticalPrescriptionCreated() { }

        #endregion Constructors

        #region Properties

        [DataMember(Name = "OccurredOn")]
        public DateTime OccurredOn { get; private set; }

        [DataMember(Name = "PrescriptionId")]
        public int PrescriptionIdentifier { get; private set; }

        #endregion Properties
    }
}
