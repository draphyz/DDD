using Conditions;
using System;
using System.Runtime.Serialization;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Domain;

    [DataContract(Namespace = "DDD.HealthcareDelivery.Domain.Prescriptions")]
    public class PharmaceuticalPrescriptionSent : IDomainEvent
    {

        #region Constructors

        public PharmaceuticalPrescriptionSent(int prescriptionIdentifier, string electronicNumber, DateTime occuredOn)
        {
            Condition.Requires(prescriptionIdentifier, nameof(prescriptionIdentifier)).IsGreaterThan(0);
            Condition.Requires(electronicNumber, nameof(electronicNumber)).IsNotNullOrWhiteSpace();
            this.PrescriptionIdentifier = prescriptionIdentifier;
            this.ElectronicNumber = electronicNumber;
            this.OccurredOn = occuredOn;
        }

        public PharmaceuticalPrescriptionSent(int prescriptionIdentifier, string electronicNumber) 
            : this(prescriptionIdentifier, electronicNumber, DateTime.Now)
        {
        }

        #endregion Constructors

        #region Properties

        [DataMember(Name = "PrescriptionId")]
        public int PrescriptionIdentifier { get; private set; }

        [DataMember(Name = "ElectronicNum")]
        public string ElectronicNumber { get; private set; }

        [DataMember(Name = "OccurredOn")]
        public DateTime OccurredOn { get; private set; }

        #endregion Properties

    }
}
