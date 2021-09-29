using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;

namespace DDD.Core.Infrastructure.Data
{
    public abstract class StoredEventMapping : ClassMapping<StoredEvent>
    {

        #region Constructors

        protected StoredEventMapping()
        {
            this.Lazy(false);
            // Table
            this.Table("Event");
            // Keys
            this.Id(e => e.Id, m1 =>  
            {
                m1.Column("EventId");
                m1.Generator(Generators.Sequence, m2 => m2.Params(new { sequence = "EventId" }));
            });
            // Fields
            this.Property(e => e.EventType, m =>
            {
                m.Type(NHibernateUtil.AnsiString);
                m.Length(50);
                m.NotNullable(true);
            });
            this.Property(e => e.Version);
            this.Property(e => e.StreamId, m =>
            {
                m.Type(NHibernateUtil.AnsiString);
                m.Length(50);
            });
            this.Property(e => e.UniqueId, m => m.NotNullable(true));
            this.Property(e => e.OccurredOn, m => m.Precision(3)); // in milliseconds
            this.Property(e => e.Username, m =>
            {
                m.Type(NHibernateUtil.AnsiString);
                m.Length(100);
            });
            this.Property(e => e.Body, m =>
            {
                m.Type(NHibernateUtil.AnsiString);
                m.NotNullable(true);
            });
            this.Property(e => e.IsDispatched);
        }

        #endregion Constructors

    }
}
