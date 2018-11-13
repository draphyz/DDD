using System.Text;

namespace DDD.Core.Serialization
{
    public interface ITextSerializer : ISerializer
    {
        #region Properties

        Encoding Encoding { get; }

        bool Indent { get; }

        #endregion Properties
    }
}
