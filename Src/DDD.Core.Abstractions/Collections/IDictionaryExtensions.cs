using System.Collections;
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

        public static bool TryGetValue<TKey, TValue>(this IDictionary<TKey, object> dictionary, TKey key, out TValue value)
        {
            Condition.Requires(dictionary, nameof(dictionary)).IsNotNull();
            if (dictionary.ContainsKey(key))
            {
                value = (TValue)dictionary[key];
                return true;
            }
            value = default;
            return false;
        }

        public static bool TryGetValue<TKey, TValue>(this IDictionary dictionary, TKey key, out TValue value)
        {
            Condition.Requires(dictionary, nameof(dictionary)).IsNotNull();
            if (dictionary.Contains(key))
            {
                value = (TValue)dictionary[key];
                return true;
            }
            value = default;
            return false;
        }

        #endregion Methods

    }
}
