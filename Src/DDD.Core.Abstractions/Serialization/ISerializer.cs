using System;
using System.IO;

namespace DDD.Serialization
{
    public interface ISerializer
    {

        #region Properties

        SerializationFormat Format { get; }

        #endregion Properties

        #region Methods

        object Deserialize(Stream stream, Type type);

        void Serialize(Stream stream, object obj);

        #endregion Methods
    }
}