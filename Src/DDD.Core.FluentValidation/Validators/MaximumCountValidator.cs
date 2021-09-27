namespace DDD.Core.Infrastructure.Validation.Validators
{
    internal class MaximumCountValidator<T> : CountValidator<T>
    {

        #region Constructors

        public MaximumCountValidator(int max) : base(0, max)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "MaximumCountValidator";

        #endregion Properties

        #region Methods

        protected override string GetDefaultMessageTemplate(string errorCode) 
            => "'{PropertyName}' must contain no more than {Max} item(s). '{PropertyName}' has {Count} item(s).";

        #endregion Methods
    }
}
