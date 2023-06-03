using System;

namespace DDD.Mapping
{
    /// <summary>
    /// Defines a method that translates an input object of one type into an output object of a different type.
    /// </summary>
    public interface IObjectTranslator
    {

        #region Properties

        Type SourceType { get; }

        Type DestinationType { get; }
        

        #endregion Properties

        #region Methods

        object Translate(object source, IMappingContext context);

        #endregion Methods

    }
}
