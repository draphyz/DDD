using System.Text;

namespace DDD.Serialization
{
    public static class JsonSerializationOptions
    {

        #region Properties

        public static Encoding Encoding { get; set; } = new UTF8Encoding(false);

        public static bool Indent { get; set; } = false;

        #endregion Properties

    }
}
