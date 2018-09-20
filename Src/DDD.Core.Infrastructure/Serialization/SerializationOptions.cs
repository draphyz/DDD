using System.Text;

namespace DDD.Core.Infrastructure.Serialization
{
    public static class SerializationOptions
    {

        #region Properties

        public static Encoding Encoding { get; set; } = Encoding.Unicode;

        public static bool Indent { get; set; } = true;

        #endregion Properties
    }
}
