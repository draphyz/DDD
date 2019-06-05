namespace DDD.Common.Domain
{
    public abstract class CountryCode : IdentificationCode
    {

        #region Constructors

        protected CountryCode() { }

        protected CountryCode(string value) : base(value)
        {
        }

        #endregion Constructors

    }
}
