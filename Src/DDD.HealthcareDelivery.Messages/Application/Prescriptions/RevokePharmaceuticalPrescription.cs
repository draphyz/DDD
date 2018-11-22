namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Application;

    /// <summary>
    /// Encapsulates all information needed to revoke a pharmaceutical prescription.
    /// </summary>
    public class RevokePharmaceuticalPrescription : ICommand
    {

        #region Properties

        public int PrescriptionIdentifier { get; set; }

        public string RevocationReason { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return $"{this.GetType().Name} [prescriptionIdentifier={this.PrescriptionIdentifier}, revocationReason={RevocationReason}]";
        }

        #endregion Methods

    }
}
