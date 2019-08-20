using NHibernate.SqlTypes;
using NHibernate.Type;
using System;

namespace DDD.Core.Infrastructure.Data
{
    [Serializable]
    public class AnsiUriType : UriType
    {

        #region Constructors

        public AnsiUriType() : base(new AnsiStringSqlType())
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name
        {
            get { return "AnsiUri"; }
        }

        #endregion Properties

    }
}
