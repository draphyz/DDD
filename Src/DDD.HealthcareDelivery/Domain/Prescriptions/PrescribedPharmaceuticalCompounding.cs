namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Domain;

    /// <summary>
    /// Represents a pharmaceutical compounding.
    /// </summary>
    public class PrescribedPharmaceuticalCompounding : PrescribedMedication
    {

        #region Constructors

        public PrescribedPharmaceuticalCompounding(string nameOrDescription,
                                                   string posology = null,
                                                   byte? quantity = null,
                                                   int identifier = 0,
                                                   EntityState entityState = EntityState.Added)
            : base(nameOrDescription, posology, quantity, null, identifier, entityState)
        {
        }

        #endregion Constructors

        #region Methods

        public override PrescribedMedicationState ToState()
        {
            var state = base.ToState();
            state.MedicationType = "Compounding";
            return state;
        }

        #endregion Methods

    }
}