using System.Collections.Generic;

namespace DDD.Mapping
{
    /// <summary>
    /// Defines a method that translates an input object of one type into an output object of another type.
    /// </summary>
    public interface IObjectTranslator<in TSource, out TDestination>
        where TSource : class
        where TDestination : class
    {
        #region Methods

        TDestination Translate(TSource source, IDictionary<string, object> options = null);

        #endregion Methods
    }
}
