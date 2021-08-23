namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Domain;

    public class PrescribedMedicationState : IStateEntity
    {

        #region Properties

        public string Code { get; set; }

        public EntityState EntityState { get; set; }

        public int Identifier { get; set; }

        public string MedicationType { get; set; }

        public string NameOrDescription { get; set; }

        public string Posology { get; set; }

        public int PrescriptionIdentifier { get; set; }

        public byte? Quantity { get; set; }

        #endregion Properties

    }
}
