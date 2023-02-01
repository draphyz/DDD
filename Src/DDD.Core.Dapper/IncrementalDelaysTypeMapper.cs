using Dapper;
using System;
using System.Linq;
using System.Collections.Generic;

namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using System.Data;

    public class IncrementalDelaysTypeMapper : SqlMapper.TypeHandler<ICollection<IncrementalDelay>>
    {

        #region Methods

        public override void SetValue(IDbDataParameter parameter, ICollection<IncrementalDelay> value)
        {
            if (value == null || !value.Any())
                parameter.Value = DBNull.Value;
            else
                parameter.Value = string.Join(",", value.Select(d => ToString(d)));
        }

        public override ICollection<IncrementalDelay> Parse(object value)
        {
            if (value is DBNull || string.IsNullOrWhiteSpace((string)value)) return new List<IncrementalDelay>();
            var delays = ((string)value).Replace(" ", "").Split(',');
            return delays.Select(d => ToIncrementalDelay(d)).ToList();
        }

        private static string ToString(IncrementalDelay delay)
        {
            return delay.Increment == 0 ? delay.Delay.ToString() : $"{delay.Delay}/{delay.Increment}";
        }

        private static IncrementalDelay ToIncrementalDelay(string delay)
        {
            var parts = delay.Split('/');
            var incrementalDelay = new IncrementalDelay { Delay = short.Parse(parts[0]) };
            if (parts.Length == 2)
                incrementalDelay.Increment = short.Parse(parts[1]);
            return incrementalDelay;
        }

        #endregion Methods

    }
}
