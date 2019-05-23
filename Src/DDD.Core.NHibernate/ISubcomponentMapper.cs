using NHibernate.Type;
using System;
using System.Linq.Expressions;

namespace DDD.Core.Infrastructure.Data
{
    public interface ISubcomponentMapper<TSubcomponent>
    {

        #region Methods

        void DiscriminatorValue(object value);

        void Property(string notVisiblePropertyOrFieldName, IType persistentType = null);

        void Property<TProperty>(Expression<Func<TSubcomponent, TProperty>> property, IType persistentType = null);

        #endregion Methods
    }
}