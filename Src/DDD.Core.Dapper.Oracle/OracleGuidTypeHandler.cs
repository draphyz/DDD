using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace DDD.Core.Infrastructure.Data
{
    /// <summary>
    /// Conversion between <see cref="Guid"/> and RAW(16) Oracle data type.
    /// </summary>
    public class OracleGuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {

        #region Methods

        public override Guid Parse(object value)
        {
            return new Guid((byte[])value);
        }

        public override void SetValue(IDbDataParameter parameter, Guid value)
        {
            OracleParameter oracleParameter = (OracleParameter)parameter;
            oracleParameter.OracleDbType = OracleDbType.Raw;
            oracleParameter.Size = 16;
            parameter.Value = value.ToByteArray();
        }

        #endregion Methods

    }
}
