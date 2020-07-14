using Conditions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Text;

namespace DDD.Core.Infrastructure.Serialization
{
    using DDD.Serialization;

    public class JsonSerializerWrapper : IJsonSerializer
    {

        #region Fields

        private readonly JsonSerializer serializer;

        #endregion Fields

        #region Constructors

        public JsonSerializerWrapper()
        {
            this.serializer = DefaultSerializer();
        }

        private JsonSerializerWrapper(JsonSerializer serializer, Encoding encoding)
        {
            Condition.Requires(serializer, nameof(serializer)).IsNotNull();
            Condition.Requires(encoding, nameof(encoding)).IsNotNull();
            this.serializer = serializer;
        }

        #endregion Constructors

        #region Properties

        public Encoding Encoding { get; } = JsonSerializationOptions.Encoding;

        public bool Indent => this.serializer.Formatting == Formatting.Indented;

        #endregion Properties

        #region Methods

        public static JsonSerializerWrapper Create(Encoding encoding, bool indent = true)
        {
            Condition.Requires(encoding, nameof(encoding)).IsNotNull();
            var serializer = DefaultSerializer();
            serializer.Formatting = indent ? Formatting.Indented : Formatting.None;
            return new JsonSerializerWrapper(serializer, encoding);
        }

        public T Deserialize<T>(Stream stream)
        {

            Condition.Requires(stream, nameof(stream)).IsNotNull();
            using (var reader = new StreamReader(stream, this.Encoding))
            using (var jsonReader = new JsonTextReader(reader))
            {
                try
                {
                    return this.serializer.Deserialize<T>(jsonReader);
                }
                catch(JsonException exception)
                {
                    throw new SerializationException(typeof(T), exception);
                }
            }
        }

        public void Serialize(Stream stream, object obj)
        {
            Condition.Requires(stream, nameof(stream)).IsNotNull();
            using (StreamWriter writer = new StreamWriter(stream, this.Encoding))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
            {
                try
                {
                    this.serializer.Serialize(jsonWriter, obj);
                }
                catch (JsonException exception)
                {
                    throw new SerializationException(obj?.GetType(), exception);
                }
            }
        }

        private static JsonSerializer DefaultSerializer()
        {
            var serializer = new JsonSerializer
            {
                Formatting = JsonSerializationOptions.Indent ? Formatting.Indented : Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            serializer.Converters.Add(new StringEnumConverter());
            return serializer;
        }

        #endregion Methods

    }
}
