using Conditions;
using System.Text;
using System.IO;

namespace DDD.Core.Serialization
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

        public static T DeserializeFromString<T>(this ISerializer serializer, string input, Encoding encoding)
        {
            Condition.Requires(serializer, nameof(serializer)).IsNotNull();
            Condition.Requires(encoding, nameof(encoding)).IsNotNull();
            var bytes = encoding.GetBytes(input);
            using (var stream = new MemoryStream(bytes))
            {
                return serializer.Deserialize<T>(stream);
            }
        }

        public static T DeserializeFromString<T>(this ISerializer serializer, string input)
        {
            return serializer.DeserializeFromString<T>(input, SerializationOptions.Encoding);
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

        public static string SerializeToString(this ISerializer serializer, object obj, Encoding encoding)
        {
            Condition.Requires(serializer, nameof(serializer)).IsNotNull();
            Condition.Requires(encoding, nameof(encoding)).IsNotNull();
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, obj);
                var bytes = stream.ToArray();
                return encoding.GetString(bytes, 0, bytes.Length);
            }
        }

        public static string SerializeToString(this ISerializer serializer, object obj)
        {
            return serializer.SerializeToString(obj, SerializationOptions.Encoding);
        }

        #endregion Methods

    }
}
