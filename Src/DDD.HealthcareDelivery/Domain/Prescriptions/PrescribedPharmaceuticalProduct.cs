namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    /// <summary>
    /// Represents a commercial pharmaceutical product with a brand name.
    /// </summary>
    public class PrescribedPharmaceuticalProduct : PrescribedMedication
    {

        #region Constructors

        public PrescribedPharmaceuticalProduct(string nameOrDescription,
                                               string posology = null,
                                               byte? quantity = null,
                                               MedicationCode code = null,
                                               int identifier = 0)
            : base(nameOrDescription, posology, quantity, code, identifier)
        {
        }

        protected PrescribedPharmaceuticalProduct() { }

        #endregion Constructors
    }
}