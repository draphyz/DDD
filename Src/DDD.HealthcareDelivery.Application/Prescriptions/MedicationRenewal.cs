using System;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Domain.Prescriptions;

    /// <summary>
    /// Encapsulates information about a medication renewal.
    /// </summary>
    public class MedicationRenewal
    {

        #region Properties

        public string Code { get; set; }

        public string Duration { get; set; }

        public DurationUnit? FrequencyUnit { get; set; }

        public byte? FrequencyValue { get; set; }

        public PrescribedMedicationType MedicationType { get; set; }

        public string NameOrDescription { get; set; }

        public byte Number { get; set; } = 1;

        public string Posology { get; set; }

        public string Quantity { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now;

        #endregion Properties

    }
}
