namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    /// <summary>
    /// Represents a pharmaceutical compounding.
    /// </summary>
    public class PrescribedPharmaceuticalCompounding : PrescribedMedication
    {

        #region Constructors

        protected PrescribedPharmaceuticalCompounding() { }

        public PrescribedPharmaceuticalCompounding(string nameOrDescription,
                                                   string posology = null,
                                                   byte? quantity = null,
                                                   int identifier = 0)
            : base(nameOrDescription, posology, quantity, null, identifier)
        {
        }

        #endregion Constructors

    }
}