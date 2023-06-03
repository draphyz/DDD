using System.Collections.Generic;

namespace DDD.Validation
{
    /// <summary>
    /// Provides access to the validation context.
    /// </summary>
    public interface IValidationContext : IDictionary<string, object>
    {
    }
}