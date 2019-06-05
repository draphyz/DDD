namespace DDD.HealthcareDelivery.Infrastructure
{
    public class BelgianSqlServerHealthcareConfigurationTests : HealthcareConfigurationTests
    {
        protected override HealthcareConfiguration CreateConfiguration()
        {
            return new BelgianSqlServerHealthcareConfiguration(@"Data Source=(local)\SQLEXPRESS;Database=Test;Integrated Security=False;User ID=sa;Password=mathib;Pooling=false");
        }
    }
}
