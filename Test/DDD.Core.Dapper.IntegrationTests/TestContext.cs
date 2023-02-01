namespace DDD.Core.Infrastructure.Data
{
    using Domain;

    public class TestContext : BoundedContext
    {
        #region Constructors

        public TestContext() : base("TST", "Test")
        {
        }

        #endregion Constructors

    }
}
