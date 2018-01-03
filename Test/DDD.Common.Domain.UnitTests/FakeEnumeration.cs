namespace DDD.Common.Domain
{
    public class FakeEnumeration : Enumeration
    {
        #region Fields

        public static FakeEnumeration

            Fake1 = new FakeEnumeration(1, "FK1", "Fake1"),
            Fake2 = new FakeEnumeration(2, "FK2", "Fake2"),
            Fake3 = new FakeEnumeration(3, "FK3", "Fake3");

        #endregion Fields

        #region Constructors

        private FakeEnumeration(int value, string code, string name) : base(value, code, name)
        {
        }

        #endregion Constructors
    }
}