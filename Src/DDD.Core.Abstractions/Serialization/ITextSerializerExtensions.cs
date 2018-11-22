using Conditions;
using System.IO;

namespace DDD.Serialization
{
    public static class ITextSerializerExtensions
    {

        #region Methods

        public static T DeserializeFromString<T>(this ITextSerializer serializer, string input)
        {
            Condition.Requires(serializer, nameof(serializer)).IsNotNull();
            var bytes = serializer.Encoding.GetBytes(input);
            using (var stream = new MemoryStream(bytes))
            {
                return serializer.Deserialize<T>(stream);
            }
        }

        public static string SerializeToString(this ITextSerializer serializer, object obj)
        {
            Condition.Requires(serializer, nameof(serializer)).IsNotNull();
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, obj);
                var bytes = stream.ToArray();
                return serializer.Encoding.GetString(bytes, 0, bytes.Length);
            }
        }

        #endregion Methods

    }
}
