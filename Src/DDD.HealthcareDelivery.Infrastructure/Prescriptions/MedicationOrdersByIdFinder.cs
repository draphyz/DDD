using System.Collections.Generic;
using Dapper;
using System.Data;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Application.Prescriptions;
    using Core.Infrastructure.Data;

    public class MedicationOrdersByIdFinder : DbQueryHandler<FindMedicationOrdersById, IEnumerable<MedicationOrder>>
    {

        #region Constructors

        public MedicationOrdersByIdFinder(IHealthcareConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }

        #endregion Constructors

        #region Methods

        protected override IEnumerable<MedicationOrder> Execute(FindMedicationOrdersById query, IDbConnection connection)
        {
            var expressions = connection.Expressions();
            var results = new Dictionary<int, MedicationOrder>();
            connection.Query<MedicationOrder, OrderedMedication, MedicationOrder>(sql: SqlScripts.FindMedicationOrdersById.Replace("@", expressions.ParameterPrefix()),
                                                                                  map: (order, medication) => Map(results, order, medication),
                                                                                  param: new { Ids = query.Identifiers },
                                                                                  splitOn: "NameOrDescription");
            return results.Values;
        }

        private static MedicationOrder Map(Dictionary<int, MedicationOrder> results, MedicationOrder order, OrderedMedication medication)
        {
            MedicationOrder result;
            if (!results.TryGetValue(order.Identifier, out result))
            {
                result = order;
                results.Add(result.Identifier, result);
            }
            result.OrderedMedications.Add(medication);
            return result;
        }

        #endregion Methods

    }
}
