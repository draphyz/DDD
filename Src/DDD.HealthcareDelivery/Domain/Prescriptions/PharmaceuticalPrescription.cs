﻿using EnsureThat;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Collections;
    using Common.Domain;
    using Core.Domain;
    using Patients;
    using Practitioners;
    using Encounters;

    /// <summary>
    /// Represents a pharmaceutical prescription.
    /// </summary>
    public class PharmaceuticalPrescription : Prescription
    {

        #region Fields

        private readonly ISet<PrescribedMedication> prescribedMedications = new HashSet<PrescribedMedication>();

        #endregion Fields

        #region Constructors

        public PharmaceuticalPrescription(PrescriptionIdentifier identifier,
                                          HealthcarePractitioner prescriber,
                                          Patient patient,
                                          IEnumerable<PrescribedMedication> prescribedMedications,
                                          Alpha2LanguageCode languageCode,
                                          PrescriptionStatus status,
                                          DateTime createdOn,
                                          EncounterIdentifier encounterIdentifier = null,
                                          DateTime? delivrableAt = null,
                                          EntityState entityState = EntityState.Added,
                                          IEnumerable<IDomainEvent> events = null)
            : base(identifier, prescriber, patient, languageCode, status, createdOn, encounterIdentifier, delivrableAt, entityState, events)
        {
            Ensure.That(prescribedMedications, nameof(prescribedMedications)).IsNotNull();
            Ensure.Enumerable.HasItems(prescribedMedications, nameof(prescribedMedications));
            Ensure.Enumerable.HasNoNull(prescribedMedications, nameof(prescribedMedications));
            this.prescribedMedications.AddRange(prescribedMedications);
        }

        protected PharmaceuticalPrescription() { }

        #endregion Constructors

        #region Methods

        public static PharmaceuticalPrescription Create(PrescriptionIdentifier identifier,
                                                        HealthcarePractitioner prescriber,
                                                        Patient patient,
                                                        IEnumerable<PrescribedMedication> prescribedMedications,
                                                        DateTime createdOn,
                                                        Alpha2LanguageCode languageCode,
                                                        EncounterIdentifier encounterIdentifier = null,
                                                        DateTime? delivrableAt = null)
        {
            var prescription = new PharmaceuticalPrescription
            (
                identifier,
                prescriber,
                patient,
                prescribedMedications,
                languageCode,
                PrescriptionStatus.Created,
                createdOn,
                encounterIdentifier,
                delivrableAt
            );
            prescription.AddEvent(new PharmaceuticalPrescriptionCreated(identifier.Value, createdOn));
            return prescription;
        }

        public static PharmaceuticalPrescription Create(PrescriptionIdentifier identifier,
                                                        HealthcarePractitioner prescriber,
                                                        Patient patient,
                                                        IEnumerable<PrescribedMedication> prescribedMedications,
                                                        Alpha2LanguageCode languageCode,
                                                        EncounterIdentifier encounterIdentifier = null,
                                                        DateTime? delivrableAt = null)
        {
            return Create(identifier, prescriber, patient, prescribedMedications, SystemTime.Local(), languageCode, encounterIdentifier, delivrableAt);
        }

        public IEnumerable<PrescribedMedication> PrescribedMedications() => this.prescribedMedications.ToImmutableHashSet();

        protected override void AddPrescriptionRevokedEvent(string reason)
        {
            this.AddEvent(new PharmaceuticalPrescriptionRevoked(this.Identifier.Value, SystemTime.Local(), reason));
        }

        #endregion Methods

    }
}
