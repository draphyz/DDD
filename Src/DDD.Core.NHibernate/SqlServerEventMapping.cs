namespace DDD.Core.Infrastructure.Data
{
    public class SqlServerEventMapping : EventMapping
    {
        #region Constructors

        public SqlServerEventMapping() 
        {
            // Fields
            this.Property(e => e.Body, m => m.Column(m1 => m1.Length(8000)));
        }

        #endregion Constructors
    }
}
