using System.Data.Entity;
using System.Data.Common;
using System.Threading.Tasks;
using System.Threading;
using System.Data.Entity.Validation;

namespace DDD.Core.Infrastructure.Data
{
    public abstract class StateEntitiesContext : DbContext
    {

        #region Constructors

        static StateEntitiesContext()
        {
            Database.SetInitializer<StateEntitiesContext>(null);
        }

        protected StateEntitiesContext(DbConnection connection, bool contextOwnsConnection) 
            : base(connection, contextOwnsConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        #endregion Constructors

        #region Methods

        public void FixEntityState()
        {
            foreach (var entry in ChangeTracker.Entries<Domain.IStateEntity>())
            {
                Domain.IStateEntity entity = entry.Entity;
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
            this.SetGeneratedValues();
            try
            {
                return base.SaveChanges();
            }
            catch(DbEntityValidationException ex)
            {
                ex.AddErrorsInData();
                throw;
            }
        }

        public override Task<int> SaveChangesAsync()
        {
            this.FixEntityState();
            this.SetGeneratedValues();
            try
            {
                return base.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                ex.AddErrorsInData();
                throw;
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            this.FixEntityState();
            this.SetGeneratedValues();
            try
            {
                return base.SaveChangesAsync(cancellationToken);
            }
            catch (DbEntityValidationException ex)
            {
                ex.AddErrorsInData();
                throw;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Add(new StateEntityConvention());
        }

        protected virtual void SetGeneratedValues()
        {
        }

        #endregion Methods

    }
}