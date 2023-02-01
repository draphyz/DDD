using EnsureThat;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DDD.Common.Domain
{
    public abstract class FlagsEnumeration : Enumeration
    {

        #region Fields

        private const string flagSeperator = ", ";

        #endregion Fields

        #region Constructors

        protected FlagsEnumeration(int value, string name) : base(value, name)
        {
        }

        protected FlagsEnumeration(int value, string code, string name) : base(value, code, name)
        {
        }

        #endregion Constructors

        #region Methods

        public static IEnumerable<int> AllFlagValues<TEnum>() where TEnum : FlagsEnumeration
        {
            var complements = AllValues<TEnum>().Select(v => ~v);
            for (var i = MinFlagValue<TEnum>(); i <= MaxFlagValue<TEnum>(); i++)
            {
                var unaccountedBits = i;
                foreach (var complement in complements)
                {
                    unaccountedBits &= complement;
                    if (unaccountedBits == 0)
                    {
                        yield return i;
                        break;
                    }
                }
            }
        }

        public static int MaxFlagValue<TEnum>() where TEnum : FlagsEnumeration
        {
            var max = 0;
            foreach (var value in AllValues<TEnum>())
                max |= value;
            return max;
        }

        public static int MinFlagValue<TEnum>() where TEnum : FlagsEnumeration => MinValue<TEnum>();

        public static new TEnum ParseCode<TEnum>(string code, bool ignoreCase = false) where TEnum : FlagsEnumeration
        {
            Ensure.That(code, nameof(code)).IsNotNullOrWhiteSpace();
            if (TryParseCode<TEnum>(code, ignoreCase, out var result))
                return result;
            throw new ArgumentOutOfRangeException("code", code, null);
        }

        public static new TEnum ParseName<TEnum>(string name, bool ignoreCase = false) where TEnum : FlagsEnumeration
        {
            Ensure.That(name, nameof(name)).IsNotNullOrWhiteSpace();
            if (TryParseName<TEnum>(name, ignoreCase, out var result))
                return result;
            throw new ArgumentOutOfRangeException("name", name, null);
        }

        public static new TEnum ParseValue<TEnum>(int value) where TEnum : FlagsEnumeration
        {
            if (TryParseValue<TEnum>(value, out var result))
                return result;
            throw new ArgumentOutOfRangeException("value", value, null);
        }

        public static new bool TryParseCode<TEnum>(string code, bool ignoreCase, out TEnum result) where TEnum : FlagsEnumeration
        {
            result = default;
            if (string.IsNullOrWhiteSpace(code)) return false;
            if (Enumeration.TryParseCode(code, ignoreCase, out result)) return true;
            var flagCodes = code.Split(new[] { flagSeperator }, StringSplitOptions.None);
            if (!flagCodes.Any()) return false;
            var flags = new List<TEnum>();
            foreach (var flagCode in flagCodes)
            {
                if (Enumeration.TryParseCode(flagCode, ignoreCase, out result))
                    flags.Add(result);
                else
                    return false;
            }
            result = CombineFlags(flags);
            return true;
        }

        public static new bool TryParseName<TEnum>(string name, bool ignoreCase, out TEnum result) where TEnum : FlagsEnumeration
        {
            result = default;
            if (string.IsNullOrWhiteSpace(name)) return false;
            if (Enumeration.TryParseName(name, ignoreCase, out result)) return true;
            var flagNames = name.Split(new[] { flagSeperator }, StringSplitOptions.None);
            if (!flagNames.Any()) return false;
            var flags = new List<TEnum>();
            foreach (var flagName in flagNames)
            {
                if (Enumeration.TryParseName(flagName, ignoreCase, out result))
                    flags.Add(result);
                else
                    return false;
            }
            result = CombineFlags(flags);
            return true;
        }

        public static new bool TryParseValue<TEnum>(int value, out TEnum result) where TEnum : FlagsEnumeration
        {
            result = default;
            if (value < MinFlagValue<TEnum>() || value > MaxFlagValue<TEnum>())
                return false;
            if (Enumeration.TryParseValue(value, out result)) return true;
            var flags = new List<TEnum>();
            foreach(var flag in AllInstances<TEnum>())
            {
                if ((value & flag.Value) != 0)
                    flags.Add(flag);
            }
            result = CombineFlags(flags);
            return true;
        }

        public TEnum AddFlag<TEnum>(TEnum flag) where TEnum : FlagsEnumeration => ParseValue<TEnum>(this.Value | flag.Value);

        public bool HasFlag<TEnum>(TEnum flag) where TEnum : FlagsEnumeration => (this.Value & flag.Value) != 0;

        public TEnum RemoveFlag<TEnum>(TEnum flag) where TEnum : FlagsEnumeration => ParseValue<TEnum>(this.Value & ~flag.Value);

        public TEnum SetFlag<TEnum>(TEnum flag, bool enable) where TEnum : FlagsEnumeration => enable ? AddFlag(flag) : RemoveFlag(flag);

        public TEnum ToggleFlag<TEnum>(TEnum flag) where TEnum : FlagsEnumeration => ParseValue<TEnum>(this.Value ^ flag.Value);

        private static TEnum CombineFlags<TEnum>(IEnumerable<TEnum> flags) where TEnum : FlagsEnumeration
        {
            flags = flags.OrderBy(f => f.Value);
            using (var enumerator = flags.GetEnumerator())
            {
                enumerator.MoveNext();
                var value = enumerator.Current.Value;
                var nameBuilder = new StringBuilder(enumerator.Current.Name);
                var codeBuilder = new StringBuilder(enumerator.Current.Code);
                while(enumerator.MoveNext())
                {
                    value |= enumerator.Current.Value;
                    nameBuilder.Append(flagSeperator).Append(enumerator.Current.Name);
                    codeBuilder.Append(flagSeperator).Append(enumerator.Current.Code);
                }
                var name = nameBuilder.ToString();
                var code = codeBuilder.ToString();
                if (code != name)
                    return (TEnum)Activator.CreateInstance(typeof(TEnum), BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { value, code, name }, null, null);
                return (TEnum)Activator.CreateInstance(typeof(TEnum), BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { value, name }, null, null);
            }
        }

        #endregion Methods

    }
}
