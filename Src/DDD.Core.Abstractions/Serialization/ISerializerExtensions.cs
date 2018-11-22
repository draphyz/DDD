using Conditions;
using System.IO;

namespace DDD.Serialization
{
    public static class ISerializerExtensions
    {

        #region Methods

        public static T DeserializeFromFile<T>(this ISerializer serializer, string filePath)
        {
            Condition.Requires(serializer, nameof(serializer)).IsNotNull();
            Condition.Requires(filePath, nameof(filePath)).IsNotNullOrWhiteSpace();
            using (var stream = File.OpenRead(filePath))
            {
                return serializer.Deserialize<T>(stream);
            }
        }

        public static void SerializeToFile(this ISerializer serializer, string filePath, object obj)
        {
            Condition.Requires(serializer, nameof(serializer)).IsNotNull();
            Condition.Requires(filePath, nameof(filePath)).IsNotNullOrWhiteSpace();
            using (var stream = File.Create(filePath))
            {
                serializer.Serialize(stream, obj);
            }
        }

        #endregion Methods

    }
}
