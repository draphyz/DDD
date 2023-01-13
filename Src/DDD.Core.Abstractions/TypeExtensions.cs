using System;
using Conditions;

namespace DDD
{
    public static class TypeExtensions
    {
        #region Methods

        public static string ShortAssemblyQualifiedName(this Type type)
        {
            Condition.Requires(type, nameof(type)).IsNotNull();
            return $"{type.FullName}, {type.Assembly.GetName().Name}";
        }

        #endregion Methods
    }
}
