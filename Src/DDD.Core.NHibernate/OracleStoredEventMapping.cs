namespace DDD.Core.Infrastructure.Data
{
    public class OracleStoredEventMapping : StoredEventMapping
    {
        #region Constructors

        public OracleStoredEventMapping()
        {
            // Fields
            this.Id(e => e.Id, m => m.Column(m1 => m1.SqlType("number(19,0)")));
            this.Property(e => e.Body, m => m.Column(m1 => m1.Length(4000)));
        }

        #endregion Constructors
    }
}
