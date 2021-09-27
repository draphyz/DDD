namespace DDD.Core.Infrastructure.Validation.Validators
{
    internal class ExactCountValidator<T> : CountValidator<T>
    {

        #region Constructors

        public ExactCountValidator(int count) : base(count, count)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "ExactCountValidator";

        #endregion Properties

        #region Methods

        protected override string GetDefaultMessageTemplate(string errorCode) 
            => "'{PropertyName}' must contain {Max} item(s). '{PropertyName}' has {Count} item(s).";

        #endregion Methods
    }
}
