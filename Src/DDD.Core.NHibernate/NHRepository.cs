using EnsureThat;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Infrastructure.Data
{
    using Domain;
    using Application;
    using Mapping;
    using Threading;
    using System.Data.Common;

    public abstract class NHRepository<TContext, TDomainEntity, TIdentity> : IRepository<TDomainEntity, TIdentity>, IDisposable
        where TContext : BoundedContext
        where TDomainEntity : DomainEntity
        where TIdentity : ComparableValueObject
    {

        #region Fields

        private readonly IObjectTranslator<IEvent, Event> eventTranslator;
        private readonly IObjectTranslator<Exception, RepositoryException> exceptionTranslator = new NHRepositoryExceptionTranslator();
        private readonly ISessionFactory<TContext> sessionFactory;
        private ISession session;
        private bool disposed;

        #endregion Fields

        #region Constructors

        protected NHRepository(ISessionFactory<TContext> sessionFactory, IObjectTranslator<IEvent, Event> eventTranslator)
        {
            Ensure.That(sessionFactory, nameof(sessionFactory)).IsNotNull();
            Ensure.That(eventTranslator, nameof(eventTranslator)).IsNotNull();
            this.sessionFactory = sessionFactory;
            this.eventTranslator = eventTranslator;
        }

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public TDomainEntity Find(TIdentity identity)
        {
            Ensure.That(identity, nameof(identity)).IsNotNull();
            try
            {
                var session = this.GetSession();
                return session.Get<TDomainEntity>(identity);
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<RepositoryException>())
            {
                throw this.TranslateException(ex);
            }
        }

        public async Task<TDomainEntity> FindAsync(TIdentity identity, CancellationToken cancellationToken = default)
        {
            Ensure.That(identity, nameof(identity)).IsNotNull();
            await new SynchronizationContextRemover();
            try
            {
                var session = await this.GetSessionAsync(cancellationToken);
                return await session.GetAsync<TDomainEntity>(identity, cancellationToken);
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<RepositoryException>())
            {
                throw this.TranslateException(ex);
            }
        }

        public void Save(TDomainEntity aggregate)
        {
            Ensure.That(aggregate, nameof(aggregate)).IsNotNull();
            try
            {
                var session = this.GetSession();
                var guidGenerator = session.Connection.SequentialGuidGenerator();
                var events = ToEvents(guidGenerator, aggregate);
                session.SaveOrUpdate(aggregate);
                foreach (var @event in events)
                    session.Save(@event);
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<RepositoryException>())
            {
                throw this.TranslateException(ex);
            }
        }

        public async Task SaveAsync(TDomainEntity aggregate, CancellationToken cancellationToken = default)
        {
            Ensure.That(aggregate, nameof(aggregate)).IsNotNull();
            try
            {
                await new SynchronizationContextRemover();
                var session = await this.GetSessionAsync(cancellationToken);
                var guidGenerator = session.Connection.SequentialGuidGenerator();
                var events = ToEvents(guidGenerator, aggregate);
                await session.SaveOrUpdateAsync(aggregate, cancellationToken);
                foreach (var @event in events)
                    await session.SaveAsync(@event, cancellationToken);
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<RepositoryException>())
            {
                throw this.TranslateException(ex);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    this.session?.Dispose();
                disposed = true;
            }
        }

        protected DbConnection GetConnection()
        {
            return this.GetSession().Connection;
        }

        protected async Task<DbConnection> GetConnectionAsync(CancellationToken cancellationToken)
        {
            return (await this.GetSessionAsync(cancellationToken)).Connection;
        }

        protected RepositoryException TranslateException(Exception ex)
        {
            return this.exceptionTranslator.Translate(ex, new { EntityType = typeof(TDomainEntity) });
        }

        private ISession GetSession()
        {
            if (this.session == null)
                this.session = this.sessionFactory.CreateSession();
            return this.session;
        }

        private async Task<ISession> GetSessionAsync(CancellationToken cancellationToken)
        {
            if (this.session == null)
                this.session = await this.sessionFactory.CreateSessionAsync(cancellationToken);
            return this.session;
        }

        private IEnumerable<Event> ToEvents(IValueGenerator<Guid> guidGenerator, TDomainEntity aggregate)
        {
            var username = Thread.CurrentPrincipal?.Identity?.Name;
            return aggregate.AllEvents().Select(e =>
            {
                var context = new
                {
                    EventId = guidGenerator.Generate(),
                    StreamId = aggregate.IdentityAsString(),
                    StreamType = aggregate.GetType().Name,
                    IssuedBy = username
                };
                return this.eventTranslator.Translate(e, context);
            });
        }

        #endregion Methods

    }
}
