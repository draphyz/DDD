namespace DDD.Mapping
{
    public class MappingProcessorSettings
    {

        #region Constructors

        public MappingProcessorSettings(IObjectMapper<object, object> defaultMapper = null, 
                                        IObjectTranslator<object, object> defaultTranslator = null) 
        {
            this.DefaultMapper = defaultMapper;
            this.DefaultTranslator = defaultTranslator; 
        }

        #endregion Constructors

        #region Properties

        public IObjectMapper<object, object> DefaultMapper { get; }

        public IObjectTranslator<object, object> DefaultTranslator { get; }

        #endregion Properties

    }
}
