using System.Collections.Generic;

namespace DDD.Mapping
{
    using Collections;

    public class MappingContext
        : Dictionary<string, object>, IMappingContext
    {

        #region Constructors

        public MappingContext() 
        {
        }
        public MappingContext(IDictionary<string, object> dictionary) : base(dictionary)
        {
        }

        #endregion Constructors

        #region Methods

        public static IMappingContext FromObject(object context)
        {
            var mappingContext = new MappingContext();
            if (context != null)
                mappingContext.AddObject(context);
            return mappingContext;
        }

        #endregion Methods

    }
}
