using System.Data.Entity;
using System.Data.Common;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Infrastructure.Data;
    using Prescriptions;

    public class SqlServerHealthcareContext : HealthcareContext
    {

        #region Constructors

        public SqlServerHealthcareContext(DbConnection connection, bool contextOwnsConnection) 
            : base(connection, contextOwnsConnection)
        {
            this.UseUpperCase = false;
        }

        #endregion Constructors

        #region Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            this.AddConfigurations(modelBuilder);
        }

        private void AddConfigurations(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Configurations.Add(new SqlServerEventStateConfiguration(this.UseUpperCase));
            modelBuilder.Configurations.Add(new SqlServerPrescriptionStateConfiguration(this.UseUpperCase));
        }

        #endregion Methods

    }
}
