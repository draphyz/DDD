using Conditions;
using DDD.Core.Domain;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace DDD.Core.Infrastructure.Data
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Represents a <see cref="DbContext"/> used to store a domain model.
    /// </summary>
    public abstract class BoundedContext : DbContext
    {

        #region Constructors

        protected BoundedContext(string connectionString)
        {
            Condition.Requires(connectionString, nameof(connectionString)).IsNotNullOrWhiteSpace();
            this.ConnectionString = connectionString;
            this.ChangeTracker.LazyLoadingEnabled = false;
            this.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        #endregion Constructors

        #region Properties

        public virtual DbSet<StoredEvent> Events { get; set; }

        protected string ConnectionString { get; }

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
            modelBuilder.HasSequence<long>("EventId");
        }

        protected override sealed void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.ApplyConfigurations(modelBuilder);
            this.ApplyConventions(modelBuilder);
        }

        #endregion Methods

    }
}