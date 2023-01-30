using EnsureThat;
using System;
using System.IO;

namespace DDD.Serialization
{
    public static class ITextSerializerExtensions
    {

        #region Methods

        public static T DeserializeFromString<T>(this ITextSerializer serializer, string input)
        {
            Ensure.That(serializer, nameof(serializer)).IsNotNull();
            return (T)serializer.DeserializeFromString(input, typeof(T));
        }

        public static object DeserializeFromString(this ITextSerializer serializer, string input, Type type)
        {
            Ensure.That(serializer, nameof(serializer)).IsNotNull();
            Ensure.That(type, nameof(type)).IsNotNull();
            var bytes = serializer.Encoding.GetBytes(input);
            using (var stream = new MemoryStream(bytes))
            {
                return serializer.Deserialize(stream, type);
            }
        }

        public static string SerializeToString(this ITextSerializer serializer, object obj)
        {
            Ensure.That(serializer, nameof(serializer)).IsNotNull();
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
