using Conditions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Infrastructure.Data;
    using Domain;

    public class SqlServerHealthcareDeliveryContextWithLogging : SqlServerHealthcareDeliveryContext
    {

        #region Fields

        private readonly ILoggerFactory loggerFactory;

        #endregion Fields

        #region Constructors

        public SqlServerHealthcareDeliveryContextWithLogging(IDbConnectionProvider<HealthcareDeliveryContext> connectionProvider, ILoggerFactory loggerFactory) 
            : base(connectionProvider)
        {
            Condition.Requires(loggerFactory, nameof(loggerFactory)).IsNotNull();
            this.loggerFactory = loggerFactory;
        }

        #endregion Constructors

        #region Methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(this.loggerFactory);
        }

        #endregion Methods

    }
}
