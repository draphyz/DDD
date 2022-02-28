using NHibernate;
using NHibernate.Type;
using System;

namespace DDD.Core.Infrastructure.Data
{
    public class EnumUpperStringType : EnumStringType
    {

        #region Constructors

        public EnumUpperStringType(Type enumClass) : base(enumClass)
        {
        }

        public EnumUpperStringType(Type enumClass, int length) : base(enumClass, length)
        {
        }

        #endregion Constructors

        #region Methods

        public override object GetInstance(object code)
        {
            try
            {
                return Enum.Parse(this.ReturnedClass, code as string, true);
            }
            catch (ArgumentException ae)
            {
                throw new HibernateException(string.Format("Can't Parse {0} as {1}", code, ReturnedClass.Name), ae);
            }
        }

        public override object GetValue(object code)
        {
            var value = (string)base.GetValue(code);
            return value?.ToUpper();
        }

        #endregion Methods

    }

    [Serializable]
    public class EnumUpperStringType<T> : EnumUpperStringType
    {

        #region Fields

        private readonly string typeName;

        #endregion Fields

        #region Constructors

        public EnumUpperStringType()
            : base(typeof(T))
        {
            var type = GetType();
            typeName = type.FullName + ", " + type.Assembly.GetName().Name;
        }

        #endregion Constructors

        #region Properties

        public override string Name
        {
            get { return typeName; }
        }

        #endregion Properties

    }
}
