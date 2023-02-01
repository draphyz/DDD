using NHibernate.Dialect;
using NHibernate.Driver;
using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure
{
    [Collection("SqlServer")]
    public class BelgianSqlServerHealthcareDeliveryConfigurationTests : HealthcareDeliveryConfigurationTests<SqlServerFixture>
    {
        #region Constructors

        public BelgianSqlServerHealthcareDeliveryConfigurationTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

        #region Methods

        protected override HealthcareDeliveryConfiguration CreateConfiguration()
        {
            var configuration = new BelgianSqlServerHealthcareDeliveryConfiguration().DataBaseIntegration(db =>
            {
                db.Dialect<MsSql2012Dialect>();
                db.Driver<MicrosoftDataSqlClientDriver>();
                db.ConnectionStringName = "SqlServer";
            });
            return (HealthcareDeliveryConfiguration)configuration;
        }

        #endregion Methods
    }
}
