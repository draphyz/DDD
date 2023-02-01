using NHibernate.Mapping.ByCode;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Infrastructure.Data;

    public abstract class SqlServerHealthcareDeliveryConfiguration : HealthcareDeliveryConfiguration
    {

        #region Methods

        protected override void AddMappings(ModelMapper modelMapper)
        {
            base.AddMappings(modelMapper);
            modelMapper.AddMapping<SqlServerEventMapping>();
        }

        #endregion Methods
    }
}
