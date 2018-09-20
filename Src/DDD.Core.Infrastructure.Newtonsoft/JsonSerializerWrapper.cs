using Conditions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Text;

namespace DDD.Core.Infrastructure.Serialization
{
    public class JsonSerializerWrapper : IJsonSerializer
    {

        #region Fields

        private readonly JsonSerializer serializer;

        #endregion Fields

        #region Constructors

        public JsonSerializerWrapper()
        {
            this.serializer = new JsonSerializer
            {
                Formatting = SerializationOptions.Indent ? Formatting.Indented : Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            serializer.Converters.Add(new StringEnumConverter());
        }

        private JsonSerializerWrapper(JsonSerializer serializer)
        {
            Condition.Requires(serializer, nameof(serializer)).IsNotNull();
            this.serializer = serializer;
        }

        #endregion Constructors

        #region Properties

        public Encoding Encoding { get; set; } = SerializationOptions.Encoding;

        #endregion Properties

        #region Methods

        public static JsonSerializerWrapper Create(JsonSerializerSettings settings)
        {
            Condition.Requires(settings, nameof(settings)).IsNotNull();
            return new JsonSerializerWrapper(JsonSerializer.Create(settings));
        }

        public T Deserialize<T>(Stream stream)
        {
            Condition.Requires(stream, nameof(stream)).IsNotNull();
            using (var reader = new StreamReader(stream, this.Encoding))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return this.serializer.Deserialize<T>(jsonReader);
            }
        }

        public void Serialize(Stream stream, object obj)
        {
            Condition.Requires(stream, nameof(stream)).IsNotNull();
            using (StreamWriter writer = new StreamWriter(stream, this.Encoding))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
            {
                this.serializer.Serialize(jsonWriter, obj);
            }
        }

        #endregion Methods

    }
}
