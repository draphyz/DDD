namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    /// <summary>
    /// Represents an active pharmaceutical substance with an International Nonproprietary Name (INN).
    /// </summary>
    public class PrescribedPharmaceuticalSubstance : PrescribedMedication
    {

        #region Constructors

        public PrescribedPharmaceuticalSubstance(string nameOrDescription,
                                                 string posology = null,
                                                 string quantity = null,
                                                 string duration = null,
                                                 MedicationCode code = null,
                                                 int identifier = 0)
            : base(nameOrDescription, posology, quantity, duration, code, identifier)
        {
        }

        protected PrescribedPharmaceuticalSubstance() { }

        #endregion Constructors
    }
}