﻿namespace DDD.Mapping
{
    /// <summary>
    /// Defines a method that maps an input object of one type to an output object of a different type.
    /// </summary>
    public interface IObjectMapper<in TSource, in TDestination>
        where TSource : class
        where TDestination : class
    {

        #region Methods

        void Map(TSource source, TDestination destination, IMappingContext context);

        #endregion Methods

    }
}
