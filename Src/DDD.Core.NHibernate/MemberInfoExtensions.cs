using System;
using System.Reflection;
using NHibernate;
using System.Linq;

namespace DDD.Core.Infrastructure.Data
{
    internal static class MemberInfoExtensions
    {

        #region Methods

        public static object GetPropertyOrFieldValue(this MemberInfo propertyOrField, object obj)
        {
            switch (propertyOrField.MemberType)
            {
                case MemberTypes.Property:
                    var property = ((PropertyInfo)propertyOrField);
                    var getter = property.GetGetMethodRecursively();
                    if (getter == null)
                        throw new PropertyNotFoundException(obj.GetType(), property.Name, "getter");
                    return getter.Invoke(obj,  new object[] { });
                case MemberTypes.Field:
                    return ((FieldInfo)propertyOrField).GetValue(obj);
                default:
                    throw new ArgumentOutOfRangeException(nameof(propertyOrField),
                                                          $"Expected PropertyInfo or FieldInfo; found :{propertyOrField.MemberType}");
            }
        }

        public static void SetPropertyOrFieldValue(this MemberInfo propertyOrField, object obj, object value)
        {
            switch(propertyOrField.MemberType)
            {
                case MemberTypes.Property:
                    var property = ((PropertyInfo)propertyOrField);
                    var setter = property.GetSetMethodRecursively();
                    if (setter == null)
                        throw new PropertyNotFoundException(obj.GetType(), property.Name, "setter");
                    setter.Invoke(obj, new [] { value });
                    break;
                case MemberTypes.Field:
                    ((FieldInfo)propertyOrField).SetValue(obj, value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(propertyOrField),
                                                          $"Expected PropertyInfo or FieldInfo; found :{propertyOrField.MemberType}");
            }
        }

        public static MethodInfo GetSetMethodRecursively(this PropertyInfo property)
        {
            var setter = property.SetMethod;
            if (setter != null) return setter;
            var reflectedType = property.ReflectedType;
            var declaringType = property.DeclaringType;
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            if (reflectedType != declaringType)
            {
                setter = declaringType.GetProperty(property.Name, bindingFlags).SetMethod;
                if (setter != null) return setter;
            }
            var interfaces = reflectedType.GetInterfaces();
            foreach(var @interface in interfaces)
            {
                setter = @interface.GetProperty(property.Name, bindingFlags)?.SetMethod;
                if (setter != null) return setter;
            }
            return null;
        }

        public static MethodInfo GetGetMethodRecursively(this PropertyInfo property)
        {
            var getter = property.GetMethod;
            if (getter != null) return getter;
            var reflectedType = property.ReflectedType;
            var declaringType = property.DeclaringType;
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            if (reflectedType != declaringType)
            {
                getter = declaringType.GetProperty(property.Name, bindingFlags).GetMethod;
                if (getter != null) return getter;
            }
            var interfaces = reflectedType.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                getter = @interface.GetProperty(property.Name, bindingFlags)?.GetMethod;
                if (getter != null) return getter;
            }
            return null;
        }




        #endregion Methods

    }
}
