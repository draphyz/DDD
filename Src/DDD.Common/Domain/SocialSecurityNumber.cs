namespace DDD.Common.Domain
{
    public abstract class SocialSecurityNumber : IdentificationNumber
    {
        #region Constructors

        protected SocialSecurityNumber(string value) : base(value)
        {
        }

        #endregion Constructors

    }
}