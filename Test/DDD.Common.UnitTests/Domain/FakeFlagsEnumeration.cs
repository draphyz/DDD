namespace DDD.Common.Domain
{
    public class FakeFlagsEnumeration : FlagsEnumeration
    {

        #region Fields

        public static FakeFlagsEnumeration

            None = new FakeFlagsEnumeration(0, "FFK0", nameof(None)),
            Fake1 = new FakeFlagsEnumeration(1, "FFK1", nameof(Fake1)),
            Fake2 = new FakeFlagsEnumeration(2, "FFK2", nameof(Fake2)),
            Fake3 = new FakeFlagsEnumeration(4, "FFK3", nameof(Fake3)),
            Fake4 = new FakeFlagsEnumeration(8, "FFK4", nameof(Fake4));

        #endregion Fields

        #region Constructors

        private FakeFlagsEnumeration(int value, string code, string name) : base(value, code, name)
        {
        }

        #endregion Constructors

        #region Methods

        // For tests
        public static FakeFlagsEnumeration Create(int value, string code, string name)
        {
            return new FakeFlagsEnumeration(value, code, name);
        }

        #endregion Methods
    }
}