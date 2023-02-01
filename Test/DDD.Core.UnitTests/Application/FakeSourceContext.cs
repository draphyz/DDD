namespace DDD.Core.Application
{
    using Domain;

    public class FakeSourceContext : BoundedContext
    {
        #region Constructors

        public FakeSourceContext() : base("FKS", "FakeSource") { }

        #endregion Constructors


    }
}
