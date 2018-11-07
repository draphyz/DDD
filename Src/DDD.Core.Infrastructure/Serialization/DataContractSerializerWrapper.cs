using Conditions;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace DDD.Core.Infrastructure.Serialization
{
    using Core.Serialization;

    public class DataContractSerializerWrapper : IXmlSerializer
    {

        #region Constructors

        public DataContractSerializerWrapper()
        {
            this.WriterSettings = new XmlWriterSettings
            {
                Encoding = SerializationOptions.Encoding,
                Indent = SerializationOptions.Indent
            };
            this.ReaderSettings = new XmlReaderSettings();
        }

        private DataContractSerializerWrapper(XmlWriterSettings writerSettings,
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

        public static DataContractSerializerWrapper Create(XmlWriterSettings writerSettings,
                                                           XmlReaderSettings readerSettings)
        {
            return new DataContractSerializerWrapper(writerSettings, readerSettings);
        }
        public T Deserialize<T>(Stream stream)
        {
            Condition.Requires(stream, nameof(stream)).IsNotNull();
            using (var reader = XmlReader.Create(stream, this.ReaderSettings))
            {
                var serializer = new DataContractSerializer(typeof(T));
                return (T)serializer.ReadObject(reader);
            }
        }

        public void Serialize(Stream stream, object obj)
        {
            Condition.Requires(stream, nameof(stream)).IsNotNull();
            Condition.Requires(obj, nameof(obj)).IsNotNull();
            using (var writer = XmlWriter.Create(stream, this.WriterSettings))
            {
                var serializer = new DataContractSerializer(obj.GetType());
                serializer.WriteObject(writer, obj);
            }
        }

        #endregion Methods

    }
}
