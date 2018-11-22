using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    public class PharmaceuticalPrescriptionState : PrescriptionState
    {

        #region Properties

        public ICollection<PrescribedMedicationState> PrescribedMedications { get; set; } = new HashSet<PrescribedMedicationState>();

        #endregion Properties

    }
}