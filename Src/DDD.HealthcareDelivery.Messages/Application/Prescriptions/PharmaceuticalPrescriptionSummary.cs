using System;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    public class PharmaceuticalPrescriptionSummary
    {

        #region Properties

        public DateTime CreatedOn { get; set; }

        public DateTime? DeliverableAt { get; set; }

        public int Identifier { get; set; }

        public string PrescriberDisplayName { get; set; }

        public PrescriptionStatus Status { get; set; }

        #endregion Properties

    }
}
