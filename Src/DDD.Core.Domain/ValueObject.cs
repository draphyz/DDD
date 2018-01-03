using System;
using System.Collections.Generic;
using System.Linq;

namespace DDD.Core.Domain
{
    using Collections;

    public abstract class ValueObject : IEquatable<ValueObject>
    {
        #region Methods

        public static bool operator !=(ValueObject a, ValueObject b)
        {
            if (object.ReferenceEquals(a, null)) return !Object.ReferenceEquals(b, null);
            return !a.Equals(b);
        }

        public static bool operator ==(ValueObject a, ValueObject b)
        {
            if (object.ReferenceEquals(a, null)) return Object.ReferenceEquals(b, null);
            return a.Equals(b);
        }

        public abstract IEnumerable<object> EqualityComponents();

        public bool Equals(ValueObject other)
        {
            if (object.ReferenceEquals(this, other)) return true;
            if (object.ReferenceEquals(other, null)) return false;
            if (this.GetType() != other.GetType()) return false;
            return this.EqualityComponents().SequenceEqual(other.EqualityComponents());
        }

        public override bool Equals(object other) => this.Equals(other as ValueObject);

        public override int GetHashCode() => this.EqualityComponents().CombineHashCodes();

        #endregion Methods
    }
}