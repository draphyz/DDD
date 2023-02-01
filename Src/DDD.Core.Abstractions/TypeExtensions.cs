using System;
using EnsureThat;

namespace DDD
{
    public static class TypeExtensions
    {
        #region Methods

        public static string ShortAssemblyQualifiedName(this Type type)
        {
            Ensure.That(type, nameof(type)).IsNotNull();
            return $"{type.FullName}, {type.Assembly.GetName().Name}";
        }

        #endregion Methods
    }
}
