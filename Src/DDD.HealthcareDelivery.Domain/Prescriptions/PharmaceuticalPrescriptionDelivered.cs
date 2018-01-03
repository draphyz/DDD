using Conditions;
using System;
using System.Runtime.Serialization;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Domain;

    [DataContract(Namespace = "DDD.HealthcareDelivery.Domain.Prescriptions")]
    public class PharmaceuticalPrescriptionDelivered : IDomainEvent
    {

        #region Constructors

        public PharmaceuticalPrescriptionDelivered(int prescriptionIdentifier, DateTime occuredOn)
        {
            Condition.Requires(prescriptionIdentifier, nameof(prescriptionIdentifier)).IsGreaterThan(0);
            this.PrescriptionIdentifier = prescriptionIdentifier;
            this.OccurredOn = occuredOn;
        }

        public PharmaceuticalPrescriptionDelivered(int prescriptionIdentifier) 
            : this(prescriptionIdentifier, DateTime.Now)
        {
        }

        #endregion Constructors

        #region Properties

        [DataMember(Name = "PrescriptionId")]
        public int PrescriptionIdentifier { get; private set; }

        [DataMember(Name = "OccurredOn")]
        public DateTime OccurredOn { get; private set; }

        #endregion Properties

    }
}
