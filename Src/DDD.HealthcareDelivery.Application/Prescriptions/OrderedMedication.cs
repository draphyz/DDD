namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    public class OrderedMedication
    {
        public string NameOrDescription { get; set; }

        public string Posology { get; set; }

        public string Duration { get; set; }

        public string Quantity { get; set; }
    }
}
