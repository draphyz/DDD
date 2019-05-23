using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Type;
using System;
using System.Data.Common;
using NHibernate;

namespace DDD.Common.Infrastructure.Data
{
    using Domain;

    public class EnumerationCodeType<T> : PrimitiveType where T : Enumeration
    {

        #region Fields

        private readonly object defaultValue;
        private readonly string name;

        #endregion Fields

        #region Constructors

        public EnumerationCodeType() : base(SqlTypeFactory.GetAnsiString(255))
        {
            if (Enumeration.TryParseValue<T>(0, out var result))
                this.defaultValue = result.Code;
            else
                this.defaultValue = "0";
            this.name = this.GetType().AssemblyQualifiedName;
        }

        #endregion Constructors

        #region Properties

        public override object DefaultValue => this.defaultValue;
        public override string Name => this.name;
        public override Type PrimitiveClass => typeof(string);
        public override Type ReturnedClass => typeof(T);

        #endregion Properties

        #region Methods

        public override object Get(DbDataReader rs, int index, ISessionImplementor session)
        {
            var code = rs[index];
            if (code == DBNull.Value || code == null) return null;
            try
            {
                return Enumeration.ParseCode<T>(code.ToString());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new HibernateException($"Can't Parse {code} as {this.ReturnedClass.Name}", ex);
            }
        }

        public override object Get(DbDataReader rs, string name, ISessionImplementor session)
        {
            return this.Get(rs, rs.GetOrdinal(name), session);
        }

        public override string ObjectToSQLString(object value, Dialect dialect) => value.ToString();

        public override void Set(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            var parameter = cmd.Parameters[index];
            if (value == null)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = ((T)value).Code;
        }

        #endregion Methods

    }
}
