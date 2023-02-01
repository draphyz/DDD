﻿using EnsureThat;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using RuntimeSerializationException = System.Runtime.Serialization.SerializationException;

namespace DDD.Core.Infrastructure.Serialization
{
    using DDD.Serialization;

    public class DataContractSerializerWrapper : IXmlSerializer
    {

        #region Fields

        private readonly XmlReaderSettings readerSettings;
        private readonly XmlWriterSettings writerSettings;

        #endregion Fields

        #region Constructors

        public DataContractSerializerWrapper()
            : this(DefaultWriterSettings(), DefaultReaderSettings())
        {
        }

        public DataContractSerializerWrapper(XmlWriterSettings writerSettings,
                                             XmlReaderSettings readerSettings)
        {
            Ensure.That(writerSettings, nameof(writerSettings)).IsNotNull();
            Ensure.That(readerSettings, nameof(readerSettings)).IsNotNull();
            this.writerSettings = writerSettings;
            this.readerSettings = readerSettings;
        }

        #endregion Constructors

        #region Properties

        public Encoding Encoding => this.writerSettings.Encoding;

        public SerializationFormat Format => SerializationFormat.Xml;

        public bool Indent => this.writerSettings.Indent;

        #endregion Properties

        #region Methods

        public static DataContractSerializerWrapper Create(Encoding encoding, bool indent = true)
        {
            Ensure.That(encoding, nameof(encoding)).IsNotNull();
            var writerSettings = DefaultWriterSettings();
            writerSettings.Encoding = encoding;
            writerSettings.Indent = indent;
            var readerSettings = DefaultReaderSettings();
            return new DataContractSerializerWrapper(writerSettings, readerSettings);
        }

        public static DataContractSerializerWrapper Create(bool indent = true) => Create(XmlSerializationOptions.Encoding, indent);

        public object Deserialize(Stream stream, Type type)
        {
            Ensure.That(stream, nameof(stream)).IsNotNull();
            Ensure.That(type, nameof(type)).IsNotNull();
            using (var reader = XmlReader.Create(stream, this.readerSettings))
            {
                var serializer = new DataContractSerializer(type);
                try
                {
                    return serializer.ReadObject(reader);
                }
                catch (RuntimeSerializationException exception)
                {
                    throw new SerializationException(type, exception);
                }
            }
        }

        public void Serialize(Stream stream, object obj)
        {
            Ensure.That(stream, nameof(stream)).IsNotNull();
            Ensure.That(obj, nameof(obj)).IsNotNull();
            using (var writer = XmlWriter.Create(stream, this.writerSettings))
            {
                var serializer = new DataContractSerializer(obj.GetType());
                try
                {
                    serializer.WriteObject(writer, obj);
                }
                catch (Exception exception) when (exception is RuntimeSerializationException || exception is InvalidDataContractException)
                {
                    throw new SerializationException(obj.GetType(), exception);
                }
            }
        }

        private static XmlReaderSettings DefaultReaderSettings() => new XmlReaderSettings();

        private static XmlWriterSettings DefaultWriterSettings()
        {
            return new XmlWriterSettings
            {
                Encoding = XmlSerializationOptions.Encoding,
                Indent = XmlSerializationOptions.Indent
            };
        }

        #endregion Methods

    }
}