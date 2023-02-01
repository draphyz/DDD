using System;

namespace DDD.Core.Infrastructure.Data
{
    /// <summary>
    /// Generates sequential Guids for string data types by combining a sequential time-based part with a random part.
    /// </summary>
    public class SequentialStringGuidGenerator : SequentialBinaryGuidGenerator
    {

        #region Methods

        protected override byte[] CombineBytes(byte[] sequentialBytes, byte[] randomBytes)
        {
            var guidBytes = base.CombineBytes(sequentialBytes, randomBytes);
            if (BitConverter.IsLittleEndian) 
            {
                // Reverse the data blocks represented as integers in the GUID
                Array.Reverse(guidBytes, 0, this.Block1Length);
                Array.Reverse(guidBytes, this.Block1Length, this.Block2Length);
                Array.Reverse(guidBytes, this.Block1Length + this.Block2Length, this.Block3Length);
            }
            return guidBytes;
        }

        #endregion Methods

    }
}
