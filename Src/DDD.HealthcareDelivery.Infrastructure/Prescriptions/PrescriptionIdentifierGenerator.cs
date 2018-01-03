using System.Data;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Core.Infrastructure.Data;
    using Application.Prescriptions;

    public class PrescriptionIdentifierGenerator : DbQueryHandler<GeneratePrescriptionIdentifier, int>
    {

        #region Constructors

        public PrescriptionIdentifierGenerator(IHealthcareConnectionFactory connectionFactory) 
            : base(connectionFactory)
        {
        }

        #endregion Constructors

        #region Methods

        protected override int Execute(GeneratePrescriptionIdentifier query, IDbConnection connection)
        {
            return connection.NextValue<int>("PrescriptionId");
        }

        #endregion Methods

    }
}
