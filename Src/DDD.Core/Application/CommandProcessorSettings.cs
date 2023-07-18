namespace DDD.Core.Application
{
    using Validation;

    public class CommandProcessorSettings
    {

        #region Constructors

        public CommandProcessorSettings(IObjectValidator<ICommand> defaultValidator = null) 
        {
            this.DefaultValidator = defaultValidator;
        }

        #endregion Constructors

        #region Properties

        public IObjectValidator<ICommand> DefaultValidator { get; }

        #endregion Properties

    }
}