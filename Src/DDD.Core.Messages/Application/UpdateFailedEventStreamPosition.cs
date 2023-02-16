using System;

namespace DDD.Core.Application
{
    public class UpdateFailedEventStreamPosition : ICommand
    {

        #region Properties

        public string Id { get; set; }

        public string Type { get; set; }

        public string Source { get; set; }

        public Guid Position { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
            => $"{this.GetType().Name} [{nameof(Id)}={this.Id}, {nameof(Type)}={this.Type}, {nameof(Source)}={this.Source}, {nameof(Position)}={this.Position}]";

        #endregion Methods

    }
}