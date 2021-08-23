namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Domain;

    /// <summary>
    /// Represents an active pharmaceutical substance with an International Nonproprietary Name (INN).
    /// </summary>
    public class PrescribedPharmaceuticalSubstance : PrescribedMedication
    {

        #region Constructors

        public PrescribedPharmaceuticalSubstance(string nameOrDescription, 
                                                 string posology = null, 
                                                 byte? quantity = null, 
                                                 MedicationCode code = null,
                                                 int identifier = 0,
                                                 EntityState entityState = EntityState.Added)
            : base(nameOrDescription, posology, quantity, code, identifier, entityState)
        {
        }

        #endregion Constructors

        #region Methods

        public override PrescribedMedicationState ToState()
        {
            var state = base.ToState();
            state.MedicationType = "Substance";
            return state;
        }

        #endregion Methods

    }
}