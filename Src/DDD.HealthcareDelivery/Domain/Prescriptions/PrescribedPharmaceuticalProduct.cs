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
            state.MedicationType = "Product";
            return state;
        }

        #endregion Methods

    }
}