using System.Collections.Generic;
using System.ComponentModel;
using Conditions;

namespace DDD.Collections
{
    public static class IDictionaryExtensions
    {

        #region Methods

        public static void AddObject(this IDictionary<string, object> dictionary, object obj)
        {
            Condition.Requires(dictionary, nameof(dictionary)).IsNotNull();
            if (obj != null)
            {
                var properties = TypeDescriptor.GetProperties(obj);
                foreach (PropertyDescriptor property in properties)
                {
                    var propertyValue = property.GetValue(obj);
                    dictionary.Add(property.Name, propertyValue);
                }
            }
        }

        #endregion Methods

    }
}
