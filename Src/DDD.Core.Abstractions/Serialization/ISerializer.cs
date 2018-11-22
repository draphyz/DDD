using System.IO;

namespace DDD.Serialization
{
    public interface ISerializer
    {

        #region Methods

        T Deserialize<T>(Stream stream);

        void Serialize(Stream stream, object obj);

        #endregion Methods

    }
}