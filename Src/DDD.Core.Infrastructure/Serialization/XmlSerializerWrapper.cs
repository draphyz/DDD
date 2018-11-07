using Conditions;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DDD.Core.Infrastructure.Serialization
{
    using Core.Serialization;

    public class XmlSerializerWrapper : IXmlSerializer
    {

        #region Constructors

        public XmlSerializerWrapper()
        {
            this.WriterSettings = new XmlWriterSettings
            {
                Encoding = SerializationOptions.Encoding,
                Indent = SerializationOptions.Indent
            };
            this.ReaderSettings = new XmlReaderSettings();
        }

        private XmlSerializerWrapper(XmlWriterSettings writerSettings,
                                     XmlReaderSettings readerSettings)
        {
            Condition.Requires(writerSettings, nameof(writerSettings)).IsNotNull();
            Condition.Requires(readerSettings, nameof(readerSettings)).IsNotNull();
            this.WriterSettings = writerSettings;
            this.ReaderSettings = readerSettings;
        }

        #endregion Constructors

        #region Properties

        public XmlReaderSettings ReaderSettings { get; private set; }
        public XmlWriterSettings WriterSettings { get; private set; }

        #endregion Properties

        #region Methods

        public static XmlSerializerWrapper Create(XmlWriterSettings writerSettings,
                                                  XmlReaderSettings readerSettings)
        {
            return new XmlSerializerWrapper(writerSettings, readerSettings);
        }

        public T Deserialize<T>(Stream stream)
        {
            Condition.Requires(stream, nameof(stream)).IsNotNull();
            using (var reader = XmlReader.Create(stream, this.ReaderSettings))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(reader);
            }
        }

        public void Serialize(Stream stream, object obj)
        {
            Condition.Requires(stream, nameof(stream)).IsNotNull();
            Condition.Requires(obj, nameof(obj)).IsNotNull();
            using (var writer = XmlWriter.Create(stream, this.WriterSettings))
            {
                var serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(writer, obj);
            }
        }

        #endregion Methods

    }
}
