using Conditions;
using System;
using System.IO;

namespace DDD.Serialization
{
    public static class ISerializerExtensions
    {

        #region Methods

        public static T Deserialize<T>(this ISerializer serializer, Stream stream)
        {
            Condition.Requires(serializer, nameof(serializer)).IsNotNull();
            Condition.Requires(stream, nameof(stream)).IsNotNull();
            return (T)serializer.Deserialize(stream, typeof(T));
        }

        public static object DeserializeFromFile(this ISerializer serializer, string filePath, Type type)
        {
            Condition.Requires(serializer, nameof(serializer)).IsNotNull();
            Condition.Requires(filePath, nameof(filePath)).IsNotNullOrWhiteSpace();
            Condition.Requires(type, nameof(type)).IsNotNull();
            using (var stream = File.OpenRead(filePath))
            {
                return serializer.Deserialize(stream, type);
            }
        }

        public static T DeserializeFromFile<T>(this ISerializer serializer, string filePath)
        {
            Condition.Requires(serializer, nameof(serializer)).IsNotNull();
            Condition.Requires(filePath, nameof(filePath)).IsNotNullOrWhiteSpace();
            return (T)serializer.DeserializeFromFile(filePath, typeof(T));
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
