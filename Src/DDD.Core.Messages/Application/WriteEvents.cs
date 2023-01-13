using System.Collections.Generic;

namespace DDD.Core.Application
{
    public class WriteEvents : ICommand
    {

        #region Properties

        public ICollection<Event> Events { get; set; } = new List<Event>();

        #endregion Properties

    }
}