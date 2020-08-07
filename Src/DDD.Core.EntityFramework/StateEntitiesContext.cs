using Conditions;
using DDD.Core.Domain;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Data.Common;

namespace DDD.Core.Infrastructure.Data
{
    using Microsoft.EntityFrameworkCore;

    public abstract class StateEntitiesContext : DbContext
    {

        #region Fields

        private DbConnection connection;

        #endregion Fields

        #region Constructors

        protected StateEntitiesContext(IDbConnectionFactory connectionFactory)
        {
            Condition.Requires(connectionFactory, nameof(connectionFactory)).IsNotNull();
            this.ConnectionFactory = connectionFactory;
            this.ChangeTracker.LazyLoadingEnabled = false;
            this.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        #endregion Constructors

        #region Properties

        protected DbConnection Connection
        {
            get
            {
                if (this.connection == null)
                    this.connection = this.ConnectionFactory.CreateConnection();
                return this.connection;
            }
        }

        protected IDbConnectionFactory ConnectionFactory { get; }

        #endregion Properties

        #region Methods

        public void FixEntityState()
        {
            foreach (var entry in ChangeTracker.Entries<Domain.IStateEntity>().ToList())
            {
                IStateEntity entity = entry.Entity;
                switch (entity.EntityState)
                {
                    case Domain.EntityState.Added:
                        entry.State = EntityState.Added;
                        break;

                    case Domain.EntityState.Modified:
                        entry.State = EntityState.Modified;
                        break;

                    case Domain.EntityState.Deleted:
                        entry.State = EntityState.Deleted;
                        break;

                    default:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }

        public override int SaveChanges()
        {
            this.FixEntityState();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.FixEntityState();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            this.FixEntityState();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.FixEntityState();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected virtual void ApplyConfigurations(ModelBuilder modelBuilder)
        {
        }

        protected virtual void ApplyConventions(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyStateEntityConvention();
        }

        protected override sealed void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.ApplyConfigurations(modelBuilder);
            this.ApplyConventions(modelBuilder);
        }

        #endregion Methods

    }
}