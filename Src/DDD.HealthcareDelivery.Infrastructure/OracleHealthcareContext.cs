using System.Data.Entity;
using System.Data.Common;
using System.Reflection;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Oracle.ManagedDataAccess.Client;
    using Prescriptions;
    using Core.Infrastructure.Data;
    using Core.Domain;

    public class OracleHealthcareContext : HealthcareContext
    {

        #region Constructors

        public OracleHealthcareContext(DbConnection connection, bool contextOwnsConnection) 
            : base(connection, contextOwnsConnection)
        {
            this.UseUpperCase = true;
        }

        #endregion Constructors

        #region Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            if (this.UseUpperCase)
                modelBuilder.ApplyAllUpperCaseConventions(false, IsIgnoredProperty);
            this.AddConfigurations(modelBuilder);
        }

        private static bool IsIgnoredProperty(PropertyInfo property)
        {
            if (typeof(IStateEntity).IsAssignableFrom(property.DeclaringType) && property.Name == nameof(IStateEntity.EntityState))
                return true;
            return false;
        }

        private void AddConfigurations(DbModelBuilder modelBuilder)
        {
            var connectionBuilder = new OracleConnectionStringBuilder(this.Database.Connection.ConnectionString);
            modelBuilder.HasDefaultSchema(connectionBuilder.UserID);
            modelBuilder.Configurations.Add(new OracleStoredEventConfiguration(this.UseUpperCase));
            modelBuilder.Configurations.Add(new OraclePrescriptionStateConfiguration(this.UseUpperCase));
        }

        #endregion Methods

    }
}
