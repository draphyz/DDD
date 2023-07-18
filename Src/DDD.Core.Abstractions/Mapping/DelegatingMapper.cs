using System;
using EnsureThat;

namespace DDD.Mapping
{
    /// <summary>
    /// Adapter that converts a delegate into an object that implements the interface IObjectMapper.
    /// </summary>
    public class DelegatingMapper<TSource, TDestination> : IObjectMapper<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {

        #region Fields

        private readonly Action<TSource, TDestination, IMappingContext> mapper;

        #endregion Fields

        #region Constructors

        public DelegatingMapper(Action<TSource, TDestination, IMappingContext> mapper) 
        {
            Ensure.That(mapper, nameof(mapper)).IsNotNull();
            this.mapper = mapper;
        }

        #endregion Constructors

        #region Methods

        public void Map(TSource source, TDestination destination, IMappingContext context)
        {
            Ensure.That(source).IsNotNull();
            Ensure.That(destination).IsNotNull();
            Ensure.That(context, nameof(context)).IsNotNull();
            this.Map(source, destination, context);
        }

        #endregion Methods

    }
}
