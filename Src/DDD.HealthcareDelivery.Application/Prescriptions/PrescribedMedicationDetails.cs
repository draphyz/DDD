namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    public class PrescribedMedicationDetails
    {
        #region Properties

        public string Code { get; set; }

        public string Duration { get; set; }

        public int Identifier { get; set; }

        public string NameOrDescription { get; set; }

        public string Posology { get; set; }

        public string Quantity { get; set; }

        #endregion Properties
    }
}
