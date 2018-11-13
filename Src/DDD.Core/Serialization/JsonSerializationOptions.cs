using System.Text;

namespace DDD.Core.Serialization
{
    public static class JsonSerializationOptions
    {

        #region Properties

        public static Encoding Encoding { get; set; } = Encoding.UTF8;

        public static bool Indent { get; set; } = true;

        #endregion Properties
    }
}
