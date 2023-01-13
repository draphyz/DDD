using System.Collections;

namespace DDD.Collections
{
    /// <summary>
    /// Adds extension methods to the <see cref="IStructuralEquatable" /> interface.
    /// </summary>
    public static class IStructuralEquatableExtensions
    {
        #region Methods

        /// <summary>
        /// Determines whether an object is structurally equal to the current instance, using the default equality comparer.
        /// </summary>
        public static bool StructuralEquals<T>(this T a, T b) where T : IStructuralEquatable
        {
            return a.Equals(b, StructuralComparisons.StructuralEqualityComparer);
        }

        #endregion Methods
    }
}
