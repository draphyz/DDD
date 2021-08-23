namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Domain.Prescriptions;

    /// <summary>
    /// Encapsulates all information needed to describe a prescribed medication.
    /// </summary>
    public class PrescribedMedicationDescriptor
    {

        #region Properties

        public string Code { get; set; }

        public PrescribedMedicationType  MedicationType { get; set; }

        public string NameOrDescription { get; set; }

        public string Posology { get; set; }

        public byte? Quantity { get; set; }

        #endregion Properties

    }
}
