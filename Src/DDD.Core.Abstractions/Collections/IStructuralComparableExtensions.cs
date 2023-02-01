using System.Collections;

namespace DDD.Collections
{
    /// <summary>
    /// Adds extension methods to the <see cref="IStructuralComparable" /> interface.
    /// </summary>
    public static class IStructuralComparableExtensions
    {

        #region Methods

        /// <summary>
        /// Determines whether the current collection object precedes, occurs in the same position as, or follows another object in the sort order, using the default comparer.
        /// </summary>
        public static int StructuralCompare<T>(this T a, T b) where T : IStructuralComparable
        {
            return a.CompareTo(b, StructuralComparisons.StructuralComparer);
        }

        #endregion Methods

    }
}
