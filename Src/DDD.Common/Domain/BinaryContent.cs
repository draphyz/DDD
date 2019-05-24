using Conditions;
using System.Collections.Generic;

namespace DDD.Common.Domain
{
    using Core.Domain;

    /// <remarks>
    /// Example of overriding HashCodeComponents(), sacrificing uniqueness of hash codes for speed.
    /// </remarks>
    public class BinaryContent : ValueObject
    {

        #region Fields

        public static readonly BinaryContent Empty = new BinaryContent(new byte[0]);

        #endregion Fields

        #region Constructors

        public BinaryContent(byte[] data)
        {
            Condition.Requires(data, nameof(data)).IsNotNull();
            this.Data = data;
        }

        #endregion Constructors

        #region Properties

        public byte[] Data { get; }

        #endregion Properties

        #region Methods

        public override IEnumerable<object> EqualityComponents()
        {
            foreach (var component in this.Data)
                yield return component;
        }

        /// <remarks>
        /// The hash code is computed on the last eight bytes of the array as in the .NET implementation of Array.IStructuralEquatable.GetHashCode().
        /// </remarks>
        public override IEnumerable<object> HashCodeComponents()
        {
            var startIndex = this.Data.Length > 8 ? this.Data.Length - 8 : 0;
            for (int i = startIndex; i < this.Data.Length; i++)
                yield return this.Data[i];
        }

        public override string ToString() => $"{this.GetType().Name} [data={this.Data}]";

        #endregion Methods

    }
}
