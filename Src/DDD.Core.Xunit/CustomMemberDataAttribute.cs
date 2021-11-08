using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xunit;
using Xunit.Sdk;

namespace DDD.Core.Infrastructure.Testing
{
    [DataDiscoverer("Xunit.Sdk.MemberDataDiscoverer", "xunit.core")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CustomMemberDataAttribute : MemberDataAttributeBase
    {

        #region Constructors

        public CustomMemberDataAttribute(string memberName, params object[] parameters)
                : base(memberName, parameters)
        {
        }

        #endregion Constructors

        #region Methods

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var type = MemberType ?? testMethod.ReflectedType;
            RuntimeHelpers.RunClassConstructor(type.TypeHandle);
            return base.GetData(testMethod);
        }

        protected override object[] ConvertDataItem(MethodInfo testMethod, object item)
        {
            if (item == null)
            {
                return null;
            }

            var array = item as object[];
            if (array == null)
            {
                throw new ArgumentException(
                        $"Property {MemberName} on {MemberType ?? testMethod.ReflectedType} yielded an item that is not an object[]");
            }
            return array;
        }

        #endregion Methods

    }
}
