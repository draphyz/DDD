using System.Data.Common;

namespace DDD.Core.Infrastructure.Data
{
    /// <summary>
    /// Defines a method that creates a <see cref="DbConnection"/>. 
    /// </summary>
    public interface IDbConnectionFactory
    {
        #region Methods

        DbConnection CreateConnection();

        #endregion Methods
    }
}
