using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Type;
using System;
using System.Data.Common;

namespace DDD.Common.Infrastructure.Data
{
    using Domain;
    using NHibernate;

    public class EnumerationValueType<T> : PrimitiveType where T : Enumeration
    {

        private readonly string name;

        #region Constructors

        public EnumerationValueType() : base(SqlTypeFactory.Int32)
        {
            this.name = this.GetType().AssemblyQualifiedName;
        }

        #endregion Constructors

        #region Properties

        public override object DefaultValue => 0;
        public override string Name => this.name;
        public override Type PrimitiveClass => typeof(int);
        public override Type ReturnedClass => typeof(T);

        #endregion Properties

        #region Methods

        public override object Get(DbDataReader rs, int index, ISessionImplementor session)
        {
            var value = rs[index];
            if (value == DBNull.Value || value == null) return null;
            try
            {
                return Enumeration.ParseValue<T>(Convert.ToInt32(value));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new HibernateException($"Can't Parse {value} as {this.ReturnedClass.Name}", ex);
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
                parameter.Value = ((T)value).Value;
        }

        #endregion Methods

    }
}
