namespace DDD.Core.Application
{
    /// <summary>
    /// Encapsulates all information needed to include a partial event stream in failure into the main event stream.
    /// </summary>
    public class IncludeFailedEventStream : ICommand
    {

        #region Properties

        public string Id { get; set; }

        public string Type { get; set; }

        public string Source { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
            => $"{this.GetType().Name} [{nameof(Id)}={this.Id}, {nameof(Type)}={this.Type}, {nameof(Source)}={this.Source}]";

        #endregion Methods

    }
}