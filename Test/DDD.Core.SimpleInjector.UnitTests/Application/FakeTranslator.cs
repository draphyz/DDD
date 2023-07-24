using System;

namespace DDD.Core.Application
{
    using Domain;
    using Mapping;

    public class FakeTranslator : IObjectTranslator<FakeEvent, FakeCommand>
    {

        #region Properties

        public Type DestinationType => typeof(FakeCommand);
        public Type SourceType => typeof(FakeEvent);

        #endregion Properties

        #region Methods

        public FakeCommand Translate(FakeEvent source, IMappingContext context)
        {
            return new FakeCommand();
        }

        public object Translate(object source, IMappingContext context)
        {
            return new FakeCommand();
        }

        #endregion Methods

    }
}
