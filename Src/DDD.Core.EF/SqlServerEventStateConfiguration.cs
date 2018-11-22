namespace DDD.Core.Infrastructure.Data
{
    public class SqlServerEventStateConfiguration : EventStateConfiguration
    {

        #region Constructors

        public SqlServerEventStateConfiguration(bool useUpperCase) : base(useUpperCase)
        {
            // Fields
            this.Property(e => e.OccurredOn)
                .HasColumnType("datetime2");
            this.Property(e => e.Body)
                .HasColumnType("xml");
        }

        #endregion Constructors

    }
}