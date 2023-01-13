using System;

namespace DDD.Core.Infrastructure.Data
{
    public class RandomGuidGenerator : GuidGenerator
    {

        #region Methods

        public override Guid Generate() => Guid.NewGuid();

        #endregion Methods

    }
}