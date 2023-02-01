using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Prescriptions;

    public abstract class HealthcareDeliveryConfiguration : Configuration
    {

        #region Constructors

        protected HealthcareDeliveryConfiguration()
        {
            var modelMapper = new ModelMapper();
            this.AddMappings(modelMapper);
            this.AddMapping(modelMapper.CompileMappingForAllExplicitlyAddedEntities());
        }

        #endregion Constructors

        #region Methods

        protected virtual void AddMappings(ModelMapper modelMapper)
        {
            modelMapper.AddMapping<PharmaceuticalPrescriptionMapping>();
            modelMapper.AddMapping<PrescribedPharmaceuticalProductMapping>();
        }
       

        #endregion Methods

    }
}
