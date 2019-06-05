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
                                                   string quantity = null,
                                                   string duration = null,
                                                   int identifier = 0)
            : base(nameOrDescription, posology, quantity, duration, null, identifier)
        {
        }

        #endregion Constructors

    }
}