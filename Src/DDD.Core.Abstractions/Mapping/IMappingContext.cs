using System.Collections.Generic;

namespace DDD.Mapping
{
    /// <summary>
    /// Provides access to the mapping context.
    /// </summary>
    public interface IMappingContext : IDictionary<string, object>
    {
    }
}