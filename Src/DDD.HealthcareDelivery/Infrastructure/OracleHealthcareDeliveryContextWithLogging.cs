using Conditions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DDD.HealthcareDelivery.Infrastructure
{
    public class OracleHealthcareDeliveryContextWithLogging : OracleHealthcareDeliveryContext
    {

        #region Fields

        private readonly ILoggerFactory loggerFactory;

        #endregion Fields

        #region Constructors

        public OracleHealthcareDeliveryContextWithLogging(string connectionString, ILoggerFactory loggerFactory) 
            : base(connectionString)
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
