namespace DDD.Core.Infrastructure.Data
{
    public class OracleEventMapping : EventMapping
    {
        #region Constructors

        public OracleEventMapping()
        {
            // Fields
            this.Property(e => e.Body, m => m.Column(m1 => m1.Length(4000)));
        }

        #endregion Constructors
    }
}
