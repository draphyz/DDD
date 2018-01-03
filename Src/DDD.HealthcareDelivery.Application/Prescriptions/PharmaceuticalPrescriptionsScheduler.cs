using System.Collections.Generic;
using Conditions;
using System.Linq;
using System;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Application;
    using Core.Collections;

    public class PharmaceuticalPrescriptionsScheduler
        : IQueryHandler<SchedulePharmaceuticalPrescriptions, IEnumerable<PharmaceuticalPrescriptionDescriptor>>
    {

        #region Methods

        public IEnumerable<PharmaceuticalPrescriptionDescriptor> Handle(SchedulePharmaceuticalPrescriptions query)
        {
            Condition.Requires(query, nameof(query)).IsNotNull();
            var prescriptions = new List<PharmaceuticalPrescriptionDescriptor>();
            foreach (var renewal in query.Renewals)
                AddPrescriptions(prescriptions, renewal);
            return prescriptions.OrderBy(p => p.DelivrableAt)
                                .ForEach(RemoveUnnecessaryDeliveryDate)
                                .ToList();
        }


        private static void AddOrUpdatePrescription(List<PharmaceuticalPrescriptionDescriptor> prescriptions,
                                                    DateTime deliveryDate,
                                                    MedicationRenewal renewal,
                                                    PrescribedMedicationDescriptor medication)
        {
            var prescription = prescriptions.FirstOrDefault(p => p.DelivrableAt == deliveryDate);
            if (prescription == null)
            {
                prescription = new PharmaceuticalPrescriptionDescriptor
                {
                    CreatedOn = renewal.StartDate,
                    DelivrableAt = deliveryDate
                };
                prescriptions.Add(prescription);
            }
            prescription.Medications.Add(medication);
        }

        private static void AddPrescriptions(List<PharmaceuticalPrescriptionDescriptor> prescriptions,
                                             MedicationRenewal renewal)
        {
            var deliveryDate = renewal.StartDate.Date;
            var medication = ToPrescribedMedication(renewal);
            for (var i = 0; i < renewal.Number; i++)
            {
                AddOrUpdatePrescription(prescriptions, deliveryDate, renewal, medication);
                IncrementDeliveryDate(ref deliveryDate, renewal);
            }
        }

        private static void IncrementDeliveryDate(ref DateTime deliveryDate, MedicationRenewal renewal)
        {
            switch (renewal.FrequencyUnit)
            {
                case DurationUnit.Month:
                    deliveryDate = deliveryDate.AddMonths(renewal.FrequencyValue.Value);
                    break;
                case DurationUnit.Week:
                    deliveryDate = deliveryDate.AddDays(renewal.FrequencyValue.Value * 7);
                    break;
                case null:
                    break;
                default:
                    throw new ArgumentException($"DurationUnit : {renewal.FrequencyUnit} not expected.", nameof(renewal));

            }
        }


        private static void RemoveUnnecessaryDeliveryDate(PharmaceuticalPrescriptionDescriptor prescription)
        {
            if (prescription.DelivrableAt == prescription.CreatedOn.Date)
                prescription.DelivrableAt = null;
        }


        private static PrescribedMedicationDescriptor ToPrescribedMedication(MedicationRenewal renewal)
        {
            return new PrescribedMedicationDescriptor
            {
                Code = renewal.Code,
                MedicationType = renewal.MedicationType,
                NameOrDescription = renewal.NameOrDescription,
                Posology = renewal.Posology,
                Quantity = renewal.Quantity,
                Duration = renewal.Duration
            };
        }

        #endregion Methods

    }
}
