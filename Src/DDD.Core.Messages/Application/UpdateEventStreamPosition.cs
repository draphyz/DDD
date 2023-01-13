using System;

namespace DDD.Core.Application
{
    public class UpdateEventStreamPosition : ICommand
    {

        #region Properties

        public string Type { get; set; }

        public string Source { get; set; }

        public Guid Position { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
            => $"{this.GetType().Name} [{nameof(Type)}={this.Type}, {nameof(Source)}={this.Source}, {nameof(Position)}={this.Position}]";

        #endregion Methods

    }
}