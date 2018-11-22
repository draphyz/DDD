using System.Text;

namespace DDD.Serialization
{
    public interface ITextSerializer : ISerializer
    {
        #region Properties

        Encoding Encoding { get; }

        bool Indent { get; }

        #endregion Properties
    }
}
