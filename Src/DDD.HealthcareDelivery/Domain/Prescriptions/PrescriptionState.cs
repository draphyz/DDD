using System;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Domain;
    using Patients;
    using Practitioners;
    using Facilities;

    public abstract class PrescriptionState : IStateEntity
    {

        #region Properties

        public DateTime CreatedOn { get; set; }

        public DateTime? DelivrableAt { get; set; }

        public EntityState EntityState { get; set; }

        public HealthFacilityState HealthFacility { get; set; }

        public int Identifier { get; set; }

        public string LanguageCode { get; set; }

        public PatientState Patient { get; set; }

        public HealthcarePractitionerState Prescriber { get; set; }

        public string Status { get; set; }

        #endregion Properties
    }
}
