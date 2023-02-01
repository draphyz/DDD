using System;
using System.Security.Cryptography;

namespace DDD.Core.Infrastructure.Data
{
    /// <summary>
    /// Generates sequential Guids for binary data types by combining a sequential time-based part with a random part.
    /// </summary>
    /// <remarks>
    /// Based on <see href="https://www.codeproject.com/Articles/388157/GUIDs-as-fast-primary-keys-under-multiple-database">GUIDs as fast primary keys under multiple databases</see>
    /// </remarks>
    public class SequentialBinaryGuidGenerator : GuidGenerator
    {

        #region Constructors

        public SequentialBinaryGuidGenerator()
        {
            this.SequentialLength = 8;
            this.RandomLength = this.GuidLength - this.SequentialLength;
            this.TimestampProvider = new UniqueTimestampProvider(new UniversalTimestampProvider(), TimeSpan.FromTicks(1));
        }

        #endregion Constructors

        #region Properties

        protected int RandomLength { get; }
        protected int SequentialLength { get; }
        protected ITimestampProvider TimestampProvider { get; }

        #endregion Properties

        #region Methods

        public sealed override Guid Generate()
        {
            var sequentialBytes = this.GenerateSequentialBytes();
            var randomBytes = this.GenerateRandomBytes();
            var guidBytes = this.CombineBytes(sequentialBytes, randomBytes);
            return new Guid(guidBytes);
        }

        /// <summary>
        /// Combines the sequential part with the random part of the GUID.
        /// </summary>
        protected virtual byte[] CombineBytes(byte[] sequentialBytes, byte[] randomBytes)
        {
            var guidBytes = new byte[this.GuidLength];
            sequentialBytes.CopyTo(guidBytes, 0);
            randomBytes.CopyTo(guidBytes, this.SequentialLength);
            return guidBytes;
        }

        /// <summary>
        /// Generates the random part of the GUID.
        /// </summary>
        protected virtual byte[] GenerateRandomBytes()
        {
            var randomBytes = new byte[this.RandomLength];
            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        /// <summary>
        /// Generates the sequential part of the GUID with the most significant bits first.
        /// </summary>
        protected virtual byte[] GenerateSequentialBytes()
        {
            var timestamp = this.TimestampProvider.GetTimestamp().Ticks;
            var sequentialBytes = BitConverter.GetBytes(timestamp);
            if (BitConverter.IsLittleEndian) Array.Reverse(sequentialBytes);
            return sequentialBytes;
        }

        #endregion Methods

    }
}