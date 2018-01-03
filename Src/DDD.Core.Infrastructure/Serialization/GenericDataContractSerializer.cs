using System;
using System.IO;
using Conditions;
using System.Xml;
using System.Runtime.Serialization;

namespace DDD.Core.Infrastructure.Serialization
{
    public class GenericDataContractSerializer<TBase> : ISerializer<TBase>
        where TBase : class
    {

        #region Methods

        public TBase Deserialize(string input, Type type)
        {
            Condition.Requires(input, nameof(input)).IsNotNull();
            Condition.Requires(type, nameof(type)).IsNotNull();
            var serializer = new DataContractSerializer(type);
            using (var reader = XmlReader.Create(new StringReader(input)))
            {
                return (TBase)serializer.ReadObject(reader);
            }
        }

        public T Deserialize<T>(string input) where T : TBase
        {
            Condition.Requires(input, nameof(input)).IsNotNull();
            return (T)this.Deserialize(input, typeof(T));
        }

        public string Serialize(TBase input, bool indented = false)
        {
            Condition.Requires(input, nameof(input)).IsNotNull();
            var serializer = new DataContractSerializer(input.GetType());
            using (var output = new StringWriter())
            using (var writer = new XmlTextWriter(output))
            {
                writer.Formatting = indented ? Formatting.Indented : Formatting.None;
                serializer.WriteObject(writer, input);
                return output.GetStringBuilder().ToString();
            }
        }

        #endregion Methods

    }
}
