using System;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    public class PharmaceuticalPrescriptionSummary
    {

        #region Properties

        public DateTime CreatedOn { get; set; }

        public DateTime? DelivrableAt { get; set; }

        public string ElectronicNumber { get; set; }

        public int Identifier { get; set; }

        public bool IsElectronic { get; set; }

        public string PrescriberDisplayName { get; set; }

        public PrescriptionStatus Status { get; set; }

        #endregion Properties

    }
}
