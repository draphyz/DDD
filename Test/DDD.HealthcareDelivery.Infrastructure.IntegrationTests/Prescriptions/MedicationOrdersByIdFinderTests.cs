using FluentAssertions;
using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Application.Prescriptions;
    using Core.Infrastructure.Data;

    [Trait("Category", "Integration")]
    public  abstract class MedicationOrdersByIdFinderTests<TFixture>
        where TFixture : IDbFixture<IHealthcareConnectionFactory>
    {
        #region Fields

        private readonly TFixture fixture;

        #endregion Fields

        #region Constructors

        protected MedicationOrdersByIdFinderTests(TFixture fixture)
        {
            this.fixture = fixture;
        }

        #endregion Constructors

        #region Methods

        public static IEnumerable<object[]> QueriesAndResults()
        {
            yield return new object[]
            {
                new FindMedicationOrdersById { Identifiers = new int[] { 1, 3, 2 } },
                new MedicationOrder[]
                {
                    new MedicationOrder
                    {
                        Identifier = 1,
                        OrderedMedications = new OrderedMedication[]
                        {
                            new OrderedMedication { NameOrDescription = "Latansoc Mylan Coll. 2,5 ml X 3" },
                            new OrderedMedication { NameOrDescription = "Dualkopt Coll. 10 ml" },
                        }
                    },
                    new MedicationOrder
                    {
                        Identifier = 2,
                        OrderedMedications = new OrderedMedication[]
                        {
                            new OrderedMedication { NameOrDescription = "Dualkopt Coll. 10 ml" }
                        }
                    },
                    new MedicationOrder
                    {
                        Identifier = 3,
                        OrderedMedications = new OrderedMedication[]
                        {
                            new OrderedMedication { NameOrDescription = "Latansoc Mylan Coll. 2,5 ml X 3" }
                        }
                    }
                }
            };
            yield return new object[]
            {
                new FindMedicationOrdersById { Identifiers = new int[] { 3, 1 } },
                new MedicationOrder[]
                {
                    new MedicationOrder
                    {
                        Identifier = 1,
                        OrderedMedications = new OrderedMedication[]
                        {
                            new OrderedMedication { NameOrDescription = "Latansoc Mylan Coll. 2,5 ml X 3" },
                            new OrderedMedication { NameOrDescription = "Dualkopt Coll. 10 ml" },
                        }
                    },
                    new MedicationOrder
                    {
                        Identifier = 3,
                        OrderedMedications = new OrderedMedication[]
                        {
                            new OrderedMedication { NameOrDescription = "Latansoc Mylan Coll. 2,5 ml X 3" }
                        }
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(QueriesAndResults))]
        public void Handle_WhenCalled_ReturnsValidResults(FindMedicationOrdersById query, IEnumerable<MedicationOrder> expectedResults)
        {
            // Arrange
            this.fixture.ExecuteScriptFromResources("FindMedicationOrdersById");
            var handler = new MedicationOrdersByIdFinder(this.fixture.ConnectionFactory);
            // Act
            var results = handler.Handle(query);
            // Assert
            results.Should().Equal(expectedResults, (r, e) => r.Identifier == e.Identifier
                                                              && EquivalentMedicationCollection(r.OrderedMedications, e.OrderedMedications));
        }

        private static bool EquivalentMedicationCollection(IEnumerable<OrderedMedication> col1, IEnumerable<OrderedMedication> col2)
        {
            if (col1 == null && col2 == null) return true;
            if (col1 == null && col2 != null) return false;
            if (col2 == null && col1 != null) return false;
            if (col1.Count() != col2.Count()) return false;
            foreach (var medication in col2)
            {
                if (!col1.Any(m => m.NameOrDescription == medication.NameOrDescription))
                    return false;
            }
            return true;
        }

        #endregion Methods
    }
}