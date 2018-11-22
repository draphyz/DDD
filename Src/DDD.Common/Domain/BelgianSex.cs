namespace DDD.Common.Domain
{
    /// <summary>
    /// Represents the Belgian administrative values for the sex (as displayed on identity cards)
    /// </summary>
    public class BelgianSex : Sex
    {

        #region Fields

        public static readonly BelgianSex

                Unknown = new BelgianSex(0, "UK", "Unknown"),
                Female = new BelgianSex(1, "F", "Female"),
                Male = new BelgianSex(2, "M", "Male"),
                Undefined = new BelgianSex(3, "UD", "Undefined"),
                Changed = new BelgianSex(4, "C", "Changed");

        #endregion Fields

        #region Constructors

        private BelgianSex(int value, string code, string name) : base(value, code, name)
        {
        }

        #endregion Constructors

    }
}