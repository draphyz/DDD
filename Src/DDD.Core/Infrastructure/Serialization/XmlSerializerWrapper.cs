using Conditions;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DDD.Core.Infrastructure.Serialization
{
    using DDD.Serialization;

    public class XmlSerializerWrapper : IXmlSerializer
    {

        #region Fields

        private readonly XmlReaderSettings readerSettings;
        private readonly XmlWriterSettings writerSettings;

        #endregion Fields

        #region Constructors

        public XmlSerializerWrapper()
        {
            this.writerSettings = new XmlWriterSettings
            {
                Encoding = XmlSerializationOptions.Encoding,
                Indent = XmlSerializationOptions.Indent
            };
            this.readerSettings = new XmlReaderSettings();
        }

        public XmlSerializerWrapper(XmlWriterSettings writerSettings,
                                    XmlReaderSettings readerSettings)
        {
            Condition.Requires(writerSettings, nameof(writerSettings)).IsNotNull();
            Condition.Requires(readerSettings, nameof(readerSettings)).IsNotNull();
            this.writerSettings = writerSettings;
            this.readerSettings = readerSettings;
        }

        #endregion Constructors

        #region Properties

        public Encoding Encoding => this.writerSettings.Encoding;

        public bool Indent => this.writerSettings.Indent;

        #endregion Properties

        #region Methods

        public static XmlSerializerWrapper Create(Encoding encoding, bool indent = true)
        {
            Condition.Requires(encoding, nameof(encoding)).IsNotNull();
            var writerSettings = new XmlWriterSettings
            {
                Encoding = encoding,
                Indent = indent
            };
            var readerSettings = new XmlReaderSettings();
            return new XmlSerializerWrapper(writerSettings, readerSettings);
        }

        public T Deserialize<T>(Stream stream)
        {
            Condition.Requires(stream, nameof(stream)).IsNotNull();
            using (var reader = XmlReader.Create(stream, this.readerSettings))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(reader);
            }
        }

        public void Serialize(Stream stream, object obj)
        {
            Condition.Requires(stream, nameof(stream)).IsNotNull();
            Condition.Requires(obj, nameof(obj)).IsNotNull();
            using (var writer = XmlWriter.Create(stream, this.writerSettings))
            {
                var serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(writer, obj);
            }
        }

        #endregion Methods

    }
}
