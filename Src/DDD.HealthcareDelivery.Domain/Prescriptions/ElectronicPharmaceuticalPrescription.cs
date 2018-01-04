using System;
using System.Collections.Generic;
using Conditions;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Domain;
    using Common.Domain;
    using Providers;
    using Patients;
    using Facilities;

    public class ElectronicPharmaceuticalPrescription : PharmaceuticalPrescription, IElectronicPrescription
    {

        #region Constructors

        public ElectronicPharmaceuticalPrescription(PrescriptionIdentifier identifier, 
                                                    HealthcareProvider prescriber, 
                                                    Patient patient, 
                                                    HealthFacility healthFacility,
                                                    IEnumerable<PrescribedMedication> prescribedMedications,
                                                    Alpha2LanguageCode languageCode,
                                                    PrescriptionStatus status,
                                                    DateTime createdOn,
                                                    DateTime? delivrableAt = null,
                                                    ElectronicPrescriptionNumber electronicNumber = null, 
                                                    EntityState entityState = EntityState.Added, 
                                                    IEnumerable<IDomainEvent> events = null) 
            : base(identifier, prescriber, patient, healthFacility, prescribedMedications, languageCode, status, createdOn, delivrableAt, entityState, events)
        {
            this.ElectronicNumber = electronicNumber;
        }

        #endregion Constructors

        #region Properties

        protected ElectronicPrescriptionNumber ElectronicNumber { get; private set; }

        #endregion Properties

        #region Methods

        public new static ElectronicPharmaceuticalPrescription Create(PrescriptionIdentifier identifier,
                                                                      HealthcareProvider prescriber,
                                                                      Patient patient,
                                                                      HealthFacility healthFacility,
                                                                      IEnumerable<PrescribedMedication> prescribedMedications,
                                                                      DateTime createdOn,
                                                                      Alpha2LanguageCode languageCode,
                                                                      DateTime? delivrableAt = null)
        {
            var prescription = new ElectronicPharmaceuticalPrescription
            (
                identifier,
                prescriber,
                patient,
                healthFacility,
                prescribedMedications,
                languageCode,
                PrescriptionStatus.Created,
                createdOn,
                delivrableAt
            );
            prescription.AddEvent(new PharmaceuticalPrescriptionCreated(identifier.Identifier, true, createdOn));
            return prescription;
        }

        public new static ElectronicPharmaceuticalPrescription Create(PrescriptionIdentifier identifier,
                                                                      HealthcareProvider prescriber,
                                                                      Patient patient,
                                                                      HealthFacility healthFacility,
                                                                      IEnumerable<PrescribedMedication> prescribedMedications,
                                                                      Alpha2LanguageCode languageCode,
                                                                      DateTime? delivrableAt = null)
        {
            return Create(identifier, prescriber, patient, healthFacility, prescribedMedications, DateTime.Now, languageCode, delivrableAt);
        }

        public void Send(ElectronicPrescriptionNumber number)
        {
            Condition.Requires(number, nameof(number)).IsNotNull();
            if (!this.IsSent())
            {
                this.ElectronicNumber = number;
                this.MarkAsModified();
                this.AddEvent(new PharmaceuticalPrescriptionSent(this.Identifier.Identifier, number.Number));
            }
        }

        public override PharmaceuticalPrescriptionState ToState()
        {
            var state = base.ToState();
            state.IsElectronic = true;
            state.ElectronicNumber = this.ElectronicNumber?.Number;
            return state;
        }

        protected bool IsSent() => this.ElectronicNumber != null;

        #endregion Methods

    }
}
