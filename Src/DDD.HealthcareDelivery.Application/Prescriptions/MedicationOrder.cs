using System;
using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    /// <summary>
    /// Encapsulates all information needed to create a medication order form.
    /// </summary>
    public class MedicationOrder
    {

        #region Properties

        public DateTime CreatedOn { get; set; }

        public DateTime? DelivrableAt { get; set; }

        public string ElectronicNumber { get; set; }

        public string HealthFacilityName { get; set; }

        public int Identifier { get; set; }

        public bool IsElectronic { get; set; }

        public string LanguageCode { get; set; }

        public ICollection<OrderedMedication> OrderedMedications { get; set; } = new List<OrderedMedication>();

        public string PatientFirstName { get; set; }

        public string PatientLastName { get; set; }

        public string PrescriberBoxNumber { get; set; }

        public string PrescriberCity { get; set; }

        public string PrescriberDisplayName { get; set; }

        public string PrescriberFirstName { get; set; }

        public string PrescriberHouseNumber { get; set; }

        public string PrescriberLastName { get; set; }

        public string PrescriberLicenseNumber { get; set; }

        public string PrescriberPostalCode { get; set; }

        public string PrescriberEmailAddress1 { get; set; }

        public string PrescriberTelephoneNumber1 { get; set; }

        public string PrescriberEmailAddress2 { get; set; }

        public string PrescriberTelephoneNumber2 { get; set; }

        public string PrescriberSpeciality { get; set; }

        public string PrescriberStreet { get; set; }

        public string PrescriberWebSite { get; set; }

        public bool StampCentred { get; set; }

        public bool StampWithCity { get; set; } = true;

        public bool StampWithHealthFacilityName { get; set; }

        public bool StampWithEmailAddress1 { get; set; }

        public bool StampWithTelephoneNumber1 { get; set; } = true;

        public bool StampWithEmailAddress2 { get; set; }

        public bool StampWithTelephoneNumber2 { get; set; }

        public bool StampWithSpeciality { get; set; } = true;

        public bool StampWithStreet { get; set; } = true;

        public bool StampWithWebSite { get; set; }

        #endregion Properties

    }
}
