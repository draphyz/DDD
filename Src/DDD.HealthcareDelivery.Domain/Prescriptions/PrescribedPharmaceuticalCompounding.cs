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
                                                   string quantity = null,
                                                   string duration = null,
                                                   int identifier = 0,
                                                   EntityState entityState = EntityState.Added)
            : base(nameOrDescription, posology, quantity, duration, null, identifier, entityState)
        {
        }

        #endregion Constructors

        #region Methods

        public override byte? QuantityAsByte()
        {
            if (this.Quantity == null) return 1;
            return base.QuantityAsByte();
        }

        public override PrescribedMedicationState ToState()
        {
            var state = base.ToState();
            state.MedicationType = PrescribedMedicationType.Compounding.ToString();
            return state;
        }

        #endregion Methods

    }
}