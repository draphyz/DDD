namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Domain;

    public class PrescribedMedicationState : IStateEntity
    {

        #region Properties

        public string Code { get; set; }

        public string Duration { get; set; }

        public EntityState EntityState { get; set; }

        public int Identifier { get; set; }

        public string MedicationType { get; set; }

        public string NameOrDescription { get; set; }

        public string Posology { get; set; }

        public int PrescriptionIdentifier { get; set; }

        public string Quantity { get; set; }

        #endregion Properties

    }
}
