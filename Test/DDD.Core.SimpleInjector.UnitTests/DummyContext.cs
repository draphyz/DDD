namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Domain;

    public class DummyContext : BoundedContext
    {
        #region Constructors

        public DummyContext() : base("DUM", "Dummy")
        {
        }

        #endregion Constructors
    }
}
