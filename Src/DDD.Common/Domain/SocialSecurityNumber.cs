namespace DDD.Common.Domain
{
    public abstract class SocialSecurityNumber : IdentificationNumber
    {
        #region Constructors

        protected SocialSecurityNumber(string number) : base(number)
        {
        }

        #endregion Constructors

    }
}