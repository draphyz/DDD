using EnsureThat;
using System;
using System.Collections.Generic;

namespace DDD.Core.Domain
{
    /// <summary>
    /// Represents a bounded context.
    /// </summary>
    public class BoundedContext : IEquatable<BoundedContext>
    {

        #region Constructors

        protected BoundedContext(string code, string name) 
        {
            Ensure.That(code, nameof(code)).IsNotNullOrWhiteSpace();
            Ensure.That(name, nameof(name)).IsNotNullOrWhiteSpace();
            Code = code;
            Name = name;
        }

        #endregion Constructors

        #region Properties

        public string Code { get; }
        public string Name { get; }

        #endregion Properties

        #region Methods

        public static bool operator !=(BoundedContext left, BoundedContext right)
        {
            return !(left == right);
        }

        public static bool operator ==(BoundedContext left, BoundedContext right)
        {
            return EqualityComparer<BoundedContext>.Default.Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BoundedContext);
        }

        public bool Equals(BoundedContext other)
        {
            return !(other is null) &&
                   Code == other.Code &&
                   Name == other.Name;
        }

        public override int GetHashCode()
        {
            return CombineHashCodes(Code, Name);
        }

        private static int CombineHashCodes(params object[] collection)
        {
            Ensure.That(collection, nameof(collection)).IsNotNull();
            unchecked
            {
                var hash = 17;
                foreach (var obj in collection)
                    hash = hash * 23 + (obj != null ? obj.GetHashCode() : 0);
                return hash;
            }
        }

        #endregion Methods

    }
}
