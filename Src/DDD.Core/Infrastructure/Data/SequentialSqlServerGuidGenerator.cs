namespace DDD.Core.Infrastructure.Data
{
    /// <summary>
    /// Generates sequential Guids for the Microsoft Sql Server uniqueidentifier data type by combining a sequential time-based part with a random part.
    /// </summary>
    /// <remarks>
    /// The comparison is made by looking at byte "groups" right-to-left, and left-to-right within a byte "group". 
    /// A byte group is what is delimited by the '-' character. 
    /// More technically, we look at bytes {10 to 15} first, then {8-9}, then {6-7}, then {4-5}, and lastly {0 to 3}.
    /// </remarks>
    public class SequentialSqlServerGuidGenerator : SequentialBinaryGuidGenerator
    {
        #region Methods

        protected override byte[] CombineBytes(byte[] sequentialBytes, byte[] randomBytes)
        {
            var guidBytes = new byte[this.GuidLength];
            randomBytes.CopyTo(guidBytes, 0);
            // Block4 with least significant bytes 
            guidBytes[08] = sequentialBytes[6];
            guidBytes[09] = sequentialBytes[7];
            // Block5 with most significant bytes 
            guidBytes[10] = sequentialBytes[0];
            guidBytes[11] = sequentialBytes[1];
            guidBytes[12] = sequentialBytes[2];
            guidBytes[13] = sequentialBytes[3];
            guidBytes[14] = sequentialBytes[4];
            guidBytes[15] = sequentialBytes[5];
            return guidBytes;
        }

        #endregion Methods
    }
}
