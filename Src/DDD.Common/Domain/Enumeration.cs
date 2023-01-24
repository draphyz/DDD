using Conditions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DDD.Common.Domain
{
    using Core.Domain;

    public abstract class Enumeration : ComparableValueObject
    {

        #region Fields

        private static readonly ConcurrentDictionary<Type, Lazy<IEnumerable<Enumeration>>> cache = new ConcurrentDictionary<Type, Lazy<IEnumerable<Enumeration>>>();

        #endregion Fields

        #region Constructors

        protected Enumeration(int value, string name) : this(value, name, name)
        {
        }

        protected Enumeration(int value, string code, string name)
        {
            Condition.Requires(code, nameof(code)).IsNotNullOrWhiteSpace();
            Condition.Requires(name, nameof(name)).IsNotNullOrWhiteSpace();
            this.Value = value;
            this.Code = code;
            this.Name = name;
        }

        #endregion Constructors

        #region Properties

        public string Code { get; }
        public string Name { get; }
        public int Value { get; }

        #endregion Properties

        #region Methods

        public static TEnum ParseCode<TEnum>(string code, bool ignoreCase = false) where TEnum : Enumeration
        {
            Condition.Requires(code, nameof(code)).IsNotNullOrWhiteSpace();
            if (TryParseCode<TEnum>(code, ignoreCase, out var result))
                return result;
            throw new ArgumentOutOfRangeException("code", code, null);
        }

        public static TEnum ParseName<TEnum>(string name, bool ignoreCase = false) where TEnum : Enumeration
        {
            Condition.Requires(name, nameof(name)).IsNotNullOrWhiteSpace();
            if (TryParseName<TEnum>(name, ignoreCase, out var result))
                return result;
            throw new ArgumentOutOfRangeException("name", name, null);
        }

        public static TEnum ParseValue<TEnum>(int value) where TEnum : Enumeration
        {
            if (TryParseValue<TEnum>(value, out var result))
                return result;
            throw new ArgumentOutOfRangeException("value", value, null);
        }

        public static bool TryParseCode<TEnum>(string code, bool ignoreCase, out TEnum result) where TEnum : Enumeration
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                result = default;
                return false;
            }
            return TryParse(c => string.Compare(c.Code, code, ignoreCase) == 0, out result);
        }

        public static bool TryParseName<TEnum>(string name, bool ignoreCase, out TEnum result) where TEnum : Enumeration
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                result = default;
                return false;
            }
            return TryParse(c => string.Compare(c.Name, name, ignoreCase) == 0, out result);
        }

        public static bool TryParseValue<TEnum>(int value, out TEnum result) where TEnum : Enumeration
        {
            return TryParse(c => c.Value == value, out result);
        }

        public static IEnumerable<TEnum> AllInstances<TEnum>() where TEnum : Enumeration
        {
            var instances = cache.GetOrAdd(typeof(TEnum), t => new Lazy<IEnumerable<Enumeration>>(GetAllInstances<TEnum>)).Value;
            return (IEnumerable<TEnum>)instances;
        }

        public static IEnumerable<int> AllValues<TEnum>() where TEnum : Enumeration => AllInstances<TEnum>().Select(i => i.Value); 

        public static int MinValue<TEnum>() where TEnum : Enumeration => AllInstances<TEnum>().First().Value;

        public static int MaxValue<TEnum>() where TEnum : Enumeration => AllInstances<TEnum>().Last().Value;

        public override IEnumerable<IComparable> ComparableComponents()
        {
            yield return this.Value;
        }

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.Value;
        }

        public override string ToString()
        {
            return $"{this.GetType().Name} [value={this.Value}, code={this.Code}, name={this.Name}]";
        }
        private static IEnumerable<TEnum> GetAllInstances<TEnum>() where TEnum : Enumeration
        {
            var enumType = typeof(TEnum);
            return enumType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                           .Where(f => enumType.IsAssignableFrom(f.FieldType))
                           .Select(f => (TEnum)f.GetValue(null))
                           .OrderBy(e => e.Value);
        }

        private static bool TryParse<TEnum>(Func<TEnum, bool> predicate, out TEnum result) where TEnum : Enumeration
        {
            result = AllInstances<TEnum>().FirstOrDefault(predicate);
            return result != null;
        }

        #endregion Methods

    }
}