using Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DDD.Common.Domain
{
    using Core;
    using Core.Domain;

    public abstract class Enumeration : ComparableValueObject
    {

        #region Constructors

        protected Enumeration(int value, string name) : this(value, name, name)
        {
        }

        protected Enumeration(int value, string code, string name)
        {
            Condition.Requires(code, nameof(code)).IsNotNullOrWhiteSpace();
            Condition.Requires(name, nameof(name)).IsNotNullOrWhiteSpace();
            this.Value = value;
            this.Code = code.ToUpper();
            this.Name = name.ToTitleCase();
        }

        #endregion Constructors

        #region Properties

        public string Code { get; }
        public string Name { get; }
        public int Value { get; }

        #endregion Properties

        #region Methods

        public static TEnum FromCode<TEnum>(string code, bool ignoreCase = false) where TEnum : Enumeration
        {
            Condition.Requires(code, nameof(code)).IsNotNullOrWhiteSpace();
            var constant = Parse<TEnum>(item => string.Compare(item.Code, code, ignoreCase) == 0);
            if (constant == null) throw new ArgumentOutOfRangeException("code", code, null);
            return constant;
        }

        public static TEnum FromName<TEnum>(string name, bool ignoreCase = false) where TEnum : Enumeration
        {
            Condition.Requires(name, nameof(name)).IsNotNullOrWhiteSpace();
            var constant = Parse<TEnum>(item => string.Compare(item.Name, name, ignoreCase) == 0);
            if (constant == null) throw new ArgumentOutOfRangeException("name", name, null);
            return constant;
        }

        public static TEnum FromValue<TEnum>(int value) where TEnum : Enumeration
        {
            var constant = Parse<TEnum>(item => item.Value == value);
            if (constant == null) throw new ArgumentOutOfRangeException("value", value, null);
            return constant;
        }

        public override IEnumerable<IComparable> ComparableComponents()
        {
            yield return this.Value;
        }

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.Value;
            yield return this.Code;
        }

        public override string ToString()
        {
            return $"{this.GetType().Name} [value={this.Value}, code={this.Code}, name={this.Name}]";
        }

        public static IEnumerable<TEnum> All<TEnum>() where TEnum : Enumeration
        {
            var constantInfos = typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            return constantInfos.Select(c => c.GetValue(null)).Cast<TEnum>();
        }

        private static TEnum Parse<TEnum>(Func<TEnum, bool> predicate) where TEnum : Enumeration
        {
            return All<TEnum>().FirstOrDefault(predicate);
        }

        #endregion Methods
    }
}