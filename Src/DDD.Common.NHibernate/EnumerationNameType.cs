using NHibernate;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Type;
using System;
using System.Data.Common;

namespace DDD.Common.Infrastructure.Data
{
    using Domain;

    public class EnumerationNameType<T> : PrimitiveType where T : Enumeration
    {

        #region Fields

        private readonly object defaultValue;
        private readonly string name;

        #endregion Fields

        #region Constructors

        public EnumerationNameType() : base(SqlTypeFactory.GetAnsiString(255))
        {
            if (Enumeration.TryParseValue<T>(0, out var result))
                this.defaultValue = result.Name;
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
            var name = rs[index];
            if (name == DBNull.Value || name == null) return null;
            try
            {
                return Enumeration.ParseName<T>(name.ToString());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new HibernateException($"Can't Parse {name} as {this.ReturnedClass.Name}", ex);
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
                parameter.Value = ((T)value).Name;
        }

        #endregion Methods

    }
}
