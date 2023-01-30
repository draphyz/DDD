using EnsureThat;
using System;
using System.Runtime.Serialization;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Domain;

    [DataContract(Namespace = "http://schemas.ddd.com/healthcare-delivery")]
    public class PharmaceuticalPrescriptionCreated : IDomainEvent
    {

        #region Constructors

        public PharmaceuticalPrescriptionCreated(int prescriptionIdentifier, DateTime occurredOn)
        {
            Ensure.That(prescriptionIdentifier, nameof(prescriptionIdentifier)).IsGt(0);
            this.PrescriptionIdentifier = prescriptionIdentifier;
            this.OccurredOn = occurredOn;
        }

        /// <remarks>For serialization</remarks>
        private PharmaceuticalPrescriptionCreated() { }

        #endregion Constructors

        #region Properties

        [DataMember(Name = "PrescriptionId")]
        public int PrescriptionIdentifier { get; private set; }

        [DataMember(Name = "OccurredOn")]
        public DateTime OccurredOn { get; private set; }

        #endregion Properties

    }
}
