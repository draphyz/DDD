namespace DDD.Core.Application
{
    using Domain;
    using Mapping;

    public class FakeMapper : IObjectMapper<FakeEvent, FakeCommand>
    {
        #region Methods

        public void Map(FakeEvent source, FakeCommand destination, IMappingContext context)
        {
        }

        #endregion Methods
    }
}