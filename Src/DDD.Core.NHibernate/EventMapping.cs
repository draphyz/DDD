using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;

namespace DDD.Core.Infrastructure.Data
{
    using Application;

    public abstract class EventMapping : ClassMapping<Event>
    {

        #region Constructors

        protected EventMapping()
        {
            this.Lazy(false);
            // Table
            this.Table("Event");
            // Keys
            this.Id(e => e.EventId, m1 =>  
            {
                m1.Column("EventId");
            });
            // Fields
            this.Property(e => e.EventType, m =>
            {
                m.Type(NHibernateUtil.AnsiString);
                m.Length(250);
                m.NotNullable(true);
            });
            this.Property(e => e.OccurredOn, m => m.Precision(3)); // in milliseconds
            this.Property(e => e.Body, m =>
            {
                m.Type(NHibernateUtil.AnsiString);
                m.NotNullable(true);
            });
            this.Property(e => e.BodyFormat, m =>
            {
                m.Type(NHibernateUtil.AnsiString);
                m.Length(20);
                m.NotNullable(true);
            });
            this.Property(e => e.StreamId, m =>
            {
                m.Type(NHibernateUtil.AnsiString);
                m.Length(50);
                m.NotNullable(true);
            });
            this.Property(e => e.StreamType, m =>
            {
                m.Type(NHibernateUtil.AnsiString);
                m.Length(50);
                m.NotNullable(true);
            });
            this.Property(e => e.IssuedBy, m =>
            {
                m.Type(NHibernateUtil.AnsiString);
                m.Length(100);
            });
        }

        #endregion Constructors

    }
}
