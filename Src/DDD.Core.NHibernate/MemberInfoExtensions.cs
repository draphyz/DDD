using System;
using System.Reflection;
using NHibernate;

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
                    if (property.GetMethod == null)
                        throw new PropertyNotFoundException(obj.GetType(), property.Name, "getter");
                    return property.GetValue(obj);
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
                    if (property.SetMethod == null)
                        throw new PropertyNotFoundException(obj.GetType(), property.Name, "setter");
                    property.SetValue(obj, value);
                    break;
                case MemberTypes.Field:
                    ((FieldInfo)propertyOrField).SetValue(obj, value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(propertyOrField),
                                                          $"Expected PropertyInfo or FieldInfo; found :{propertyOrField.MemberType}");
            }
        }

        #endregion Methods

    }
}
