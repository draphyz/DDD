using EnsureThat;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Text;

namespace DDD.Core.Infrastructure.Serialization
{
    using DDD.Serialization;

    public class JsonSerializerWrapper : IJsonSerializer
    {

        #region Fields

        private readonly JsonSerializerSettings settings;

        #endregion Fields

        #region Constructors

        public JsonSerializerWrapper()
            : this(DefaultSettings(), JsonSerializationOptions.Encoding)
        {
        }

        public JsonSerializerWrapper(JsonSerializerSettings settings, Encoding encoding)
        {
            Ensure.That(settings, nameof(settings)).IsNotNull();
            Ensure.That(encoding, nameof(encoding)).IsNotNull();
            this.settings = settings;
            this.Encoding = encoding;
        }

        #endregion Constructors

        #region Properties

        public Encoding Encoding { get; }

        public SerializationFormat Format => SerializationFormat.Json;

        public bool Indent => this.settings.Formatting == Formatting.Indented;

        #endregion Properties

        #region Methods

        public static JsonSerializerWrapper Create(Encoding encoding, bool indent = true)
        {
            Ensure.That(encoding, nameof(encoding)).IsNotNull();
            var settings = DefaultSettings();
            settings.Formatting = indent ? Formatting.Indented : Formatting.None;
            return new JsonSerializerWrapper(settings, encoding);
        }

        public static JsonSerializerWrapper Create(bool indent = true) => Create(JsonSerializationOptions.Encoding, indent);

        public object Deserialize(Stream stream, Type type)
        {

            Ensure.That(stream, nameof(stream)).IsNotNull();
            Ensure.That(type, nameof(type)).IsNotNull();
            using (var streamReader = new StreamReader(stream, this.Encoding, true, 1024, true))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var serializer = JsonSerializer.Create(this.settings);
                try
                {
                    return serializer.Deserialize(jsonReader, type);
                }
                catch (JsonException exception)
                {
                    throw new SerializationException(type, exception);
                }
            }
        }

        public void Serialize(Stream stream, object obj)
        {
            Ensure.That(stream, nameof(stream)).IsNotNull();
            using (var streamWriter = new StreamWriter(stream, this.Encoding, 1024, true))
            using (var jsonWriter = new JsonTextWriter(streamWriter))
            {
                var serializer = JsonSerializer.Create(this.settings);
                try
                {
                    serializer.Serialize(jsonWriter, obj);
                }
                catch (JsonException exception)
                {
                    throw new SerializationException(obj?.GetType(), exception);
                }
            }
        }

        private static JsonSerializerSettings DefaultSettings()
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = JsonSerializationOptions.Indent ? Formatting.Indented : Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            settings.Converters.Add(new StringEnumConverter());
            return settings;
        }

        #endregion Methods

    }
}
