using System;

namespace DDD.Core.Infrastructure.Serialization
{
    public interface ISerializer<TBase>
    {

        #region Methods

        T Deserialize<T>(string input) where T : TBase;

        TBase Deserialize(string input, Type type);

        string Serialize(TBase input, bool indented = false);

        #endregion Methods

    }
}