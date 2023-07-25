using NHibernate;
using NHibernate.Cfg;
using System;
using System.Threading.Tasks;
using System.Threading;
using EnsureThat;

namespace DDD.Core.Infrastructure.Data
{
    using Domain;

    public class DelegatingSessionFactory<TContext> : ISessionFactory<TContext>, IDisposable
        where TContext : BoundedContext
    {

        #region Fields

        private readonly Func<ISessionBuilder, CancellationToken, Task> asyncOptions;
        private readonly Configuration configuration;
        private readonly Action<ISessionBuilder> options;
        private readonly Lazy<ISessionFactory> lazySessionFactory;
        private bool disposed;

        #endregion Fields

        #region Constructors

        public DelegatingSessionFactory(TContext context, 
                                        Configuration configuration, 
                                        Action<ISessionBuilder> options, 
                                        Func<ISessionBuilder, CancellationToken, Task> asyncOptions) 
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            Ensure.That(configuration, nameof(configuration)).IsNotNull();
            Ensure.That(options, nameof(options)).IsNotNull();
            Ensure.That(asyncOptions, nameof(asyncOptions)).IsNotNull();
            this.configuration = configuration;
            this.options = options;
            this.asyncOptions = asyncOptions;
            this.lazySessionFactory = new Lazy<ISessionFactory>(() => this.configuration.BuildSessionFactory());
            this.Context = context;
        }

        #endregion Constructors

        #region Properties

        public TContext Context { get; }

        private ISessionFactory SessionFactory => this.lazySessionFactory.Value;

        #endregion Properties

        #region Methods

        public static ISessionFactory<TContext> Create(TContext context, Configuration configuration, Action<ISessionBuilder> options)
        {
            Ensure.That(configuration, nameof(configuration)).IsNotNull();
            Ensure.That(options, nameof(options)).IsNotNull();
            Func<ISessionBuilder, CancellationToken, Task> asyncOptions = (b, t) => { options(b); return Task.CompletedTask; };
            return new DelegatingSessionFactory<TContext>(context, configuration, options, asyncOptions);
        }

        public ISession CreateSession()
        {
            var sessionBuilder = this.SessionFactory.WithOptions();
            this.options(sessionBuilder);
            return sessionBuilder.OpenSession();
        }

        public async Task<ISession> CreateSessionAsync(CancellationToken cancellationToken = default)
        {
            var sessionBuilder = this.SessionFactory.WithOptions();
            await this.asyncOptions(sessionBuilder, cancellationToken);
            return sessionBuilder.OpenSession();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (this.lazySessionFactory.IsValueCreated)
                        this.SessionFactory.Dispose();
                }
                disposed = true;
            }
        }

        #endregion Methods

    }
}
