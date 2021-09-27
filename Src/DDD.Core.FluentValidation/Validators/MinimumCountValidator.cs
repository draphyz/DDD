namespace DDD.Core.Infrastructure.Validation.Validators
{
    internal class MinimumCountValidator<T> : CountValidator<T>
    {

        #region Constructors

        public MinimumCountValidator(int min) : base(min, null)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "MinimumCountValidator";

        #endregion Properties

        #region Methods

        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' must contain at least {Min} item(s). '{PropertyName}' has {Count} item(s).";

        #endregion Methods
    }
}
