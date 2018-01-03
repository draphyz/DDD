using Conditions;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Domain;

    /// <summary>
    /// Represents a commercial pharmaceutical product with a brand name.
    /// </summary>
    public class PrescribedPharmaceuticalProduct : PrescribedMedication
    {

        #region Constructors

        public PrescribedPharmaceuticalProduct(string nameOrDescription, 
                                               string posology =null, 
                                               string quantity = null, 
                                               string duration = null,
                                               MedicationCode code = null,
                                               int identifier = 0,
                                               EntityState entityState = EntityState.Added)
            : base(nameOrDescription, posology, quantity, duration, code, identifier, entityState)
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
            state.MedicationType = PrescribedMedicationType.Product.ToString();
            return state;
        }

        #endregion Methods

    }
}