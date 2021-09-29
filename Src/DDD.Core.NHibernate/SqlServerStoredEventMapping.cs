namespace DDD.Core.Infrastructure.Data
{
    public class SqlServerStoredEventMapping : StoredEventMapping
    {
        #region Constructors

        public SqlServerStoredEventMapping() 
        {
            // Fields
            this.Property(e => e.Body, m => m.Column(m1 => m1.Length(8000)));
        }

        #endregion Constructors
    }
}
