﻿using System;
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
            return !(a == b);
        }

        public static bool operator ==(ValueObject a, ValueObject b)
        {
            if (a is null) return b is null;
            return a.Equals(b);
        }

        public abstract IEnumerable<object> EqualityComponents();

        public bool Equals(ValueObject other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (this.GetType() != other.GetType()) return false;
            return this.EqualityComponents().SequenceEqual(other.EqualityComponents());
        }

        public override bool Equals(object other) => this.Equals(other as ValueObject);

        public override int GetHashCode() => this.HashCodeComponents().CombineHashCodes();

        public virtual IEnumerable<object> HashCodeComponents() => this.EqualityComponents();

        public IEnumerable<object> PrimitiveEqualityComponents()
        {
            foreach(var component in this.EqualityComponents())
            {
                var valueObject = component as ValueObject;
                if (valueObject == null)
                    yield return component;
                else
                {
                    foreach (var primitiveComponent in valueObject.PrimitiveEqualityComponents())
                        yield return primitiveComponent;
                }
            }
        }

        #endregion Methods

    }
}