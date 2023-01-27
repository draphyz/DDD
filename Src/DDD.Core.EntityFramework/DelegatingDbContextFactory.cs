using Microsoft.EntityFrameworkCore;
using Conditions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Infrastructure.Data
{
    public class DelegatingDbContextFactory<TContext> : IDbContextFactory<TContext>
        where TContext : DbContext
    {

        #region Fields

        private readonly Func<DbContextOptionsBuilder<TContext>, CancellationToken, Task<TContext>> asyncFactory;
        private readonly Func<DbContextOptionsBuilder<TContext>, TContext> factory;

        #endregion Fields

        #region Constructors

        public DelegatingDbContextFactory(Func<DbContextOptionsBuilder<TContext>, TContext> factory, Func<DbContextOptionsBuilder<TContext>, CancellationToken, Task<TContext>> asyncFactory)
        {
            Condition.Requires(factory, nameof(factory)).IsNotNull();
            Condition.Requires(asyncFactory, nameof(asyncFactory)).IsNotNull();
            this.factory = factory;
            this.asyncFactory = asyncFactory;
        }

        #endregion Constructors

        #region Methods

        public static IDbContextFactory<TContext> Create(Func<DbContextOptionsBuilder<TContext>, TContext> factory)
        {
            Condition.Requires(factory, nameof(factory)).IsNotNull();
            Func<DbContextOptionsBuilder<TContext>, CancellationToken, Task<TContext>> asyncFactory = (b, t) => Task.FromResult(factory(b));
            return new DelegatingDbContextFactory<TContext>(factory, asyncFactory);
        }

        public TContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            return this.factory(optionsBuilder);
        }

        public Task<TContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            return this.asyncFactory(optionsBuilder, cancellationToken);
        }

        #endregion Methods

    }
}
