using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Core.Infrastructure.Data;
    using Application.Prescriptions;

    public class PrescriptionIdentifierGenerator : DbQueryHandler<GeneratePrescriptionIdentifier, int>
    {

        #region Constructors

        public PrescriptionIdentifierGenerator(IHealthcareDeliveryConnectionFactory connectionFactory) 
            : base(connectionFactory)
        {
        }

        #endregion Constructors

        #region Methods

        protected override Task<int> ExecuteAsync(GeneratePrescriptionIdentifier query, 
                                                  IDbConnection connection, 
                                                  CancellationToken cancellationToken = default)
        {
            return connection.NextValueAsync<int>("PrescriptionId", cancellationToken: cancellationToken);
        }

        #endregion Methods

    }
}
