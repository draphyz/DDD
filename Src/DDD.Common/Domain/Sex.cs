namespace DDD.Common.Domain
{
    public abstract class Sex : Enumeration
    {

        #region Constructors

        protected Sex(int value, string code, string name) : base(value, code, name)
        {
        }

        #endregion Constructors

    }
}