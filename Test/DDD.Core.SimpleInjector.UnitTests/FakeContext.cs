namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Domain;

    public class FakeContext : BoundedContext
    {
        #region Constructors

        public FakeContext() : base("FK", "Fake")
        {
        }

        #endregion Constructors
    }
}
