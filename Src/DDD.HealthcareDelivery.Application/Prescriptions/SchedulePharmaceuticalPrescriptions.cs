using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Application;

    /// <summary>
    /// Encapsulates all information needed to schedule pharmaceutical prescriptions based on information about medication renewals.
    /// </summary>
    public class SchedulePharmaceuticalPrescriptions : IQuery<IEnumerable<PharmaceuticalPrescriptionDescriptor>>
    {
        #region Properties

        public ICollection<MedicationRenewal> Renewals { get; set; } = new List<MedicationRenewal>();

        #endregion Properties
    }
}
