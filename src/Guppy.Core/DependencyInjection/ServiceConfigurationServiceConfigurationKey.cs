using Guppy.DependencyInjection.ServiceConfigurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    public struct ServiceConfigurationServiceConfigurationKey
    {
        public readonly IServiceConfiguration ServiceConfiguration;
        public readonly ServiceConfigurationKey ServiceConfigurationKey;

        public ServiceConfigurationServiceConfigurationKey(
            IServiceConfiguration serviceConfiguration, 
            ServiceConfigurationKey serviceConfigurationKey)
        {
            ServiceConfiguration = serviceConfiguration;
            ServiceConfigurationKey = serviceConfigurationKey;
        }


        #region Method Overloading
        public override int GetHashCode()
        {
            int hashCode = 274901523;
            hashCode = hashCode * -1521134295 + EqualityComparer<IServiceConfiguration>.Default.GetHashCode(ServiceConfiguration);
            hashCode = hashCode * -1521134295 + ServiceConfigurationKey.GetHashCode();
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return obj is ServiceConfigurationServiceConfigurationKey key &&
                   EqualityComparer<IServiceConfiguration>.Default.Equals(ServiceConfiguration, key.ServiceConfiguration) &&
                   EqualityComparer<ServiceConfigurationKey>.Default.Equals(ServiceConfigurationKey, key.ServiceConfigurationKey);
        }

        public static bool operator ==(ServiceConfigurationServiceConfigurationKey scsck1, ServiceConfigurationServiceConfigurationKey scsck2)
        {
            return scsck1.ServiceConfiguration.Key == scsck2.ServiceConfiguration.Key
                && scsck1.ServiceConfigurationKey == scsck2.ServiceConfigurationKey;
        }

        public static bool operator !=(ServiceConfigurationServiceConfigurationKey scsck1, ServiceConfigurationServiceConfigurationKey scsck2) => !(scsck1 == scsck2);
        #endregion
    }
}
