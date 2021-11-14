using System.Text;

namespace DDD.Serialization
{
    public static class XmlSerializationOptions
    {

        #region Properties

        public static Encoding Encoding { get; set; } = Encoding.UTF8;

        public static bool Indent { get; set; } = false;

        #endregion Properties
    }
}
