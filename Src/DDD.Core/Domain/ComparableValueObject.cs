using System;
using System.Collections.Generic;

namespace DDD.Core.Domain
{
    public abstract class ComparableValueObject : ValueObject, IComparable, IComparable<ComparableValueObject>
    {
        #region Methods

        public static bool operator <(ComparableValueObject a, ComparableValueObject b)
        {
            if (ReferenceEquals(a, null)) return !ReferenceEquals(b, null);
            return a.CompareTo(b) < 0;
        }

        public static bool operator <=(ComparableValueObject a, ComparableValueObject b)
        {
            if (ReferenceEquals(a, null)) return true;
            return a.CompareTo(b) <= 0;
        }

        public static bool operator >(ComparableValueObject a, ComparableValueObject b)
        {
            if (ReferenceEquals(a, null)) return false;
            return a.CompareTo(b) > 0;
        }

        public static bool operator >=(ComparableValueObject a, ComparableValueObject b)
        {
            if (ReferenceEquals(a, null)) return ReferenceEquals(b, null);
            return a.CompareTo(b) >= 0;
        }

        public abstract IEnumerable<IComparable> ComparableComponents();

        public int CompareTo(ComparableValueObject other)
        {
            if (ReferenceEquals(other, null)) return 1;
            using (var thisComponents = this.ComparableComponents().GetEnumerator())
            using (var otherComponents = other.ComparableComponents().GetEnumerator())
            {
                while (true)
                {
                    if (thisComponents.MoveNext())
                    {
                        otherComponents.MoveNext();
                        if (ReferenceEquals(thisComponents.Current, null))
                        {
                            if (!ReferenceEquals(otherComponents.Current, null)) return -1;
                        }
                        else
                        {
                            var result = thisComponents.Current.CompareTo(otherComponents.Current);
                            if (result != 0) return result;
                        }
                    }
                    else return 0;
                }
            }
        }

        int IComparable.CompareTo(object other)
        {
            var comparableValueObject = other as ComparableValueObject;
            if (comparableValueObject == null)
                throw new ArgumentException("The argument is not a ComparableValueObject.", "other");
            return this.CompareTo(comparableValueObject);
        }

        protected IComparable AsNonGenericComparable<T>(IComparable<T> comparable) where T : class
        {
            return new NonGenericComparable<T>(comparable);
        }

        #endregion Methods

        #region Classes

        private class NonGenericComparable<T> : IComparable where T : class
        {
            #region Fields

            private readonly IComparable<T> comparable;

            #endregion Fields

            #region Constructors

            public NonGenericComparable(IComparable<T> comparable)
            {
                this.comparable = comparable;
            }

            #endregion Constructors

            #region Methods

            public int CompareTo(object other)
            {
                if (ReferenceEquals(this.comparable, null))
                {
                    if (ReferenceEquals(other, null)) return 0;
                    return -1;
                }
                return this.comparable.CompareTo(other as T);
            }

            #endregion Methods
        }

        #endregion Classes
    }
}