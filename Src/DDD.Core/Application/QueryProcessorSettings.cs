namespace DDD.Core.Application
{
    using Validation;

    public class QueryProcessorSettings
    {

        #region Constructors

        public QueryProcessorSettings(IObjectValidator<IQuery> defaultValidator = null)
        {
            this.DefaultValidator = defaultValidator;
        }

        #endregion Constructors

        #region Properties

        public IObjectValidator<IQuery> DefaultValidator { get; }

        #endregion Properties

    }
}