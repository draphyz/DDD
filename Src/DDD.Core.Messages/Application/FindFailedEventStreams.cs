using System.Collections.Generic;

namespace DDD.Core.Application
{
    public class FindFailedEventStreams : IQuery<IEnumerable<FailedEventStream>>
    {
    }
}