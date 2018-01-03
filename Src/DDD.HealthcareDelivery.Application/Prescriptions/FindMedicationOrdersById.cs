using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Application;

    /// <summary>
    /// Encapsulates information needed to find medication orders based on their identifiers.
    /// </summary>
    public class FindMedicationOrdersById : IQuery<IEnumerable<MedicationOrder>>
    {

        #region Properties

        public ICollection<int> Identifiers { get; set; } = new List<int>();

        #endregion Properties

    }
}
