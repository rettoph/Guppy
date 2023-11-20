using Autofac;
using Guppy.Configurations;
using Guppy.Enums;
using System.Reflection;

namespace Guppy.Attributes
{
    public class ServiceAttribute : GuppyConfigurationAttribute
    {
        public readonly Type? ServiceType;
        public readonly ServiceLifetime Lifetime;
        public readonly bool RequireAutoLoadAttribute;

        public ServiceAttribute(ServiceLifetime lifetime, Type? serviceType, bool requireAutoLoadAttribute)
        {
            this.Lifetime = lifetime;
            this.ServiceType = serviceType;
            this.RequireAutoLoadAttribute = requireAutoLoadAttribute;
        }

        protected override bool ShouldConfigure(GuppyConfiguration configuration, Type classType)
        {
            var result =  base.ShouldConfigure(configuration, classType);

            if(this.RequireAutoLoadAttribute)
            {
                result &= classType.HasCustomAttribute<AutoLoadAttribute>();
            }

            return result;
        }

        protected override void Configure(GuppyConfiguration configuration, Type classType)
        {
            var service = configuration.Builder.RegisterType(classType).As(this.ServiceType ?? classType);

            switch (this.Lifetime)
            {
                case ServiceLifetime.Transient:
                    service.InstancePerDependency();
                    break;
                case ServiceLifetime.Scoped:
                    service.InstancePerLifetimeScope();
                    break;
                case ServiceLifetime.Singleton:
                    service.SingleInstance();
                    break;
            }
        }
    }

    public class ServiceAttribute<TService> : ServiceAttribute
    {
        public ServiceAttribute(ServiceLifetime lifetime, bool requireAutoLoadAttribute) : base(lifetime, typeof(TService), requireAutoLoadAttribute)
        {
        }
    }
}
