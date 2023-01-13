using System;

namespace DDD.Core.Infrastructure.Data
{
    /// <summary>
    /// Base class for the generators of Guids.
    /// </summary>
    public abstract class GuidGenerator : IValueGenerator<Guid>
    {

        #region Constructors

        protected GuidGenerator()
        {
            this.Block1Length = 4;
            this.Block2Length = 2;
            this.Block3Length = 2;
            this.Block4Length = 2;
            this.Block5Length = 6;
            this.GuidLength = this.Block1Length + this.Block2Length + this.Block3Length + this.Block4Length + this.Block5Length;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The length of the first block of a UUID (represented as an Int32 in the GUID).
        /// </summary>
        protected int Block1Length { get; }

        /// <summary>
        /// The length of the second block of a UUID (represented as an Int16 in the GUID).
        /// </summary>
        protected int Block2Length { get; }

        /// <summary>
        /// The length of the third block of a UUID (represented as an Int16 in the GUID).
        /// </summary>
        protected int Block3Length { get; }

        /// <summary>
        /// The length of the fourth block of a UUID (represented as bytes in the GUID).
        /// </summary>
        protected int Block4Length { get; }

        /// <summary>
        /// The length of the fifth block of a UUID (represented as bytes in the GUID).
        /// </summary>
        protected int Block5Length { get; }

        /// <summary>
        /// The total length of the GUID.
        /// </summary>
        protected int GuidLength { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Generates Guids.
        /// </summary>
        public abstract Guid Generate();

        #endregion Methods

    }
}
