using System;
using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    /// <summary>
    /// Encapsulates all information needed to describe a pharmaceutical prescription.
    /// </summary>
    public class PharmaceuticalPrescriptionDescriptor
    {

        #region Properties

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime? DelivrableAt { get; set; }

        public ICollection<PrescribedMedicationDescriptor> Medications { get; set; } 
            = new List<PrescribedMedicationDescriptor>();

        public int PrescriptionIdentifier { get; set; }

        #endregion Properties

    }
}
