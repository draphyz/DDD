using EnsureThat;
using System;
using System.IO;

namespace DDD.Serialization
{
    public static class ISerializerExtensions
    {

        #region Methods

        public static T Deserialize<T>(this ISerializer serializer, Stream stream)
        {
            Ensure.That(serializer, nameof(serializer)).IsNotNull();
            Ensure.That(stream, nameof(stream)).IsNotNull();
            return (T)serializer.Deserialize(stream, typeof(T));
        }

        public static object DeserializeFromFile(this ISerializer serializer, string filePath, Type type)
        {
            Ensure.That(serializer, nameof(serializer)).IsNotNull();
            Ensure.That(filePath, nameof(filePath)).IsNotNullOrWhiteSpace();
            Ensure.That(type, nameof(type)).IsNotNull();
            using (var stream = File.OpenRead(filePath))
            {
                return serializer.Deserialize(stream, type);
            }
        }

        public static T DeserializeFromFile<T>(this ISerializer serializer, string filePath)
        {
            Ensure.That(serializer, nameof(serializer)).IsNotNull();
            Ensure.That(filePath, nameof(filePath)).IsNotNullOrWhiteSpace();
            return (T)serializer.DeserializeFromFile(filePath, typeof(T));
        }

        public static void SerializeToFile(this ISerializer serializer, string filePath, object obj)
        {
            Ensure.That(serializer, nameof(serializer)).IsNotNull();
            Ensure.That(filePath, nameof(filePath)).IsNotNullOrWhiteSpace();
            using (var stream = File.Create(filePath))
            {
                serializer.Serialize(stream, obj);
            }
        }

        #endregion Methods

    }
}
