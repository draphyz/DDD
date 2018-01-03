using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using System;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    [Trait("Category", "Unit")]
    public class PharmaceuticalPrescriptionsSchedulerTests
    {

        public static IEnumerable<object[]> QueriesAndExpectedResults()
        {
            yield return new object[]
            {
                new SchedulePharmaceuticalPrescriptions
                {
                    Renewals = new MedicationRenewal[]
                    {
                        new MedicationRenewal
                        {
                            NameOrDescription = "A",
                            StartDate = new DateTime(2018, 9, 1, 10, 5, 1)
                        },
                        new MedicationRenewal
                        {
                            NameOrDescription = "B",
                            StartDate = new DateTime(2018, 9, 1, 10, 5, 2)
                        }
                    }
                },
                new PharmaceuticalPrescriptionDescriptor[]
                {
                    new PharmaceuticalPrescriptionDescriptor
                    {
                        CreatedOn = new DateTime(2018, 9, 1, 10, 5, 1),
                        Medications = new PrescribedMedicationDescriptor[]
                        {
                            new PrescribedMedicationDescriptor { NameOrDescription = "A" },
                            new PrescribedMedicationDescriptor { NameOrDescription = "B" }
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SchedulePharmaceuticalPrescriptions
                {
                    Renewals = new MedicationRenewal[]
                    {
                        new MedicationRenewal
                        {
                            NameOrDescription = "A",
                            StartDate = new DateTime(2018, 9, 1, 10, 5, 1),
                            Number = 2,
                            FrequencyValue = 3,
                            FrequencyUnit = DurationUnit.Month
                        },
                        new MedicationRenewal
                        {
                            NameOrDescription = "B",
                            StartDate = new DateTime(2018, 9, 1, 10, 5, 2),
                            Number = 3,
                            FrequencyValue = 1,
                            FrequencyUnit = DurationUnit.Month
                        }
                    }
                },
                new PharmaceuticalPrescriptionDescriptor[]
                {
                    new PharmaceuticalPrescriptionDescriptor
                    {
                        CreatedOn = new DateTime(2018, 9, 1, 10, 5, 1),
                        Medications = new PrescribedMedicationDescriptor[]
                        {
                            new PrescribedMedicationDescriptor { NameOrDescription = "A" },
                            new PrescribedMedicationDescriptor { NameOrDescription = "B" }
                        }
                    },
                    new PharmaceuticalPrescriptionDescriptor
                    {
                        CreatedOn = new DateTime(2018, 9, 1, 10, 5, 2),
                        DelivrableAt = new DateTime(2018, 10, 1),
                        Medications = new PrescribedMedicationDescriptor[]
                        {
                            new PrescribedMedicationDescriptor { NameOrDescription = "B" }
                        }
                    },
                    new PharmaceuticalPrescriptionDescriptor
                    {
                        CreatedOn = new DateTime(2018, 9, 1, 10, 5, 2),
                        DelivrableAt = new DateTime(2018, 11, 1),
                        Medications = new PrescribedMedicationDescriptor[]
                        {
                            new PrescribedMedicationDescriptor { NameOrDescription = "B" }
                        }
                    },
                    new PharmaceuticalPrescriptionDescriptor
                    {
                        CreatedOn = new DateTime(2018, 9, 1, 10, 5, 1),
                        DelivrableAt = new DateTime(2018, 12, 1),
                        Medications = new PrescribedMedicationDescriptor[]
                        {
                            new PrescribedMedicationDescriptor { NameOrDescription = "A" }
                        }
                    }
                }
            };
            yield return new object[]
            {
                new SchedulePharmaceuticalPrescriptions
                {
                    Renewals = new MedicationRenewal[]
                    {
                        new MedicationRenewal
                        {
                            NameOrDescription = "A",
                            StartDate = new DateTime(2018, 9, 1),
                            Number = 2,
                            FrequencyValue = 3,
                            FrequencyUnit = DurationUnit.Month
                        },
                        new MedicationRenewal
                        {
                            NameOrDescription = "B",
                            StartDate = new DateTime(2018, 10, 1),
                            Number = 3,
                            FrequencyValue = 1,
                            FrequencyUnit = DurationUnit.Month
                        }
                    }
                },
                new PharmaceuticalPrescriptionDescriptor[]
                {
                    new PharmaceuticalPrescriptionDescriptor
                    {
                        CreatedOn = new DateTime(2018, 9, 1),
                        Medications = new PrescribedMedicationDescriptor[]
                        {
                            new PrescribedMedicationDescriptor { NameOrDescription = "A" }
                        }
                    },
                    new PharmaceuticalPrescriptionDescriptor
                    {
                        CreatedOn = new DateTime(2018, 10, 1),
                        Medications = new PrescribedMedicationDescriptor[]
                        {
                            new PrescribedMedicationDescriptor { NameOrDescription = "B" }
                        }
                    },
                    new PharmaceuticalPrescriptionDescriptor
                    {
                        CreatedOn = new DateTime(2018, 10, 1),
                        DelivrableAt = new DateTime(2018, 11, 1),
                        Medications = new PrescribedMedicationDescriptor[]
                        {
                            new PrescribedMedicationDescriptor { NameOrDescription = "B" }
                        }
                    },
                    new PharmaceuticalPrescriptionDescriptor
                    {
                        CreatedOn = new DateTime(2018, 9, 1),
                        DelivrableAt = new DateTime(2018, 12, 1),
                        Medications = new PrescribedMedicationDescriptor[]
                        {
                            new PrescribedMedicationDescriptor { NameOrDescription = "A" },
                            new PrescribedMedicationDescriptor { NameOrDescription = "B" }
                        }
                    }
                }

            };
        }

        [Theory]
        [MemberData(nameof(QueriesAndExpectedResults))]
        public void Handle_WhenCalled_ReturnsExceptedResults(SchedulePharmaceuticalPrescriptions query, 
                                                             IEnumerable<PharmaceuticalPrescriptionDescriptor> expectedResults)
        {
            // Arrange
            var handler = new PharmaceuticalPrescriptionsScheduler();
            // Act
            var results = handler.Handle(query);
            // Assert 
            results.ShouldBeEquivalentTo(expectedResults);
        }

    }
}
