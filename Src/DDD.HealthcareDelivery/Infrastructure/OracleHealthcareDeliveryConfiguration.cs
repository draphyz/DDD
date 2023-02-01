﻿using NHibernate.Mapping.ByCode;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Infrastructure.Data;

    public abstract class OracleHealthcareDeliveryConfiguration : HealthcareDeliveryConfiguration
    {

        #region Constructors

        protected OracleHealthcareDeliveryConfiguration()
        {
            this.SetNamingStrategy(UpperCaseNamingStrategy.Instance);
        }

        #endregion Constructors

        #region Methods

        protected override void AddMappings(ModelMapper modelMapper)
        {
            base.AddMappings(modelMapper);
            modelMapper.AddMapping<OracleEventMapping>();
        }

        #endregion Methods

    }
}
