namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    public class PrescribedMedicationDetails
    {
        #region Properties

        public string Code { get; set; }

        public int Identifier { get; set; }

        public string NameOrDescription { get; set; }

        public string Posology { get; set; }

        public byte? Quantity { get; set; }

        #endregion Properties
    }
}
