using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Application;

    /// <summary>
    /// Encapsulates information needed to find prescribed medications associated with a prescription.
    /// </summary>
    public class FindPrescribedMedicationsByPrescription : IQuery<IEnumerable<PrescribedMedicationDetails>>
    {

        #region Properties

        public int PrescriptionIdentifier { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return $"{this.GetType().Name} [prescriptionIdentifier={this.PrescriptionIdentifier}]";
        }

        #endregion Methods

    }
}
