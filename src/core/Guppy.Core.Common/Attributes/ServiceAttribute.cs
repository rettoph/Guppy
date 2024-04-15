using Autofac;
using Guppy.Core.Common.Enums;
using System.Reflection;

namespace Guppy.Core.Common.Attributes
{
    public class ServiceAttribute : GuppyConfigurationAttribute
    {
        public readonly Type? ServiceType;
        public readonly ServiceLifetime Lifetime;
        public readonly bool RequireAutoLoadAttribute;
        public readonly object? Tag;

        public ServiceAttribute(ServiceLifetime lifetime, Type? serviceType, bool requireAutoLoadAttribute, object? tag = null)
        {
            this.Lifetime = lifetime;
            this.ServiceType = serviceType;
            this.RequireAutoLoadAttribute = requireAutoLoadAttribute;
            this.Tag = tag;
        }

        protected override bool ShouldConfigure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            var result = base.ShouldConfigure(boot, builder, classType);

            if (this.RequireAutoLoadAttribute)
            {
                result &= classType.HasCustomAttribute<AutoLoadAttribute>();
            }

            return result;
        }

        protected override void Configure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            var service = builder.RegisterType(classType).As(this.ServiceType ?? classType);

            switch (this.Lifetime)
            {
                case ServiceLifetime.Transient:
                    service.InstancePerDependency();
                    break;
                case ServiceLifetime.Scoped:
                    if (this.Tag is null)
                    {
                        service.InstancePerLifetimeScope();
                    }
                    else
                    {
                        service.InstancePerMatchingLifetimeScope(this.Tag);
                    }

                    break;
                case ServiceLifetime.Singleton:
                    service.SingleInstance();
                    break;
            }
        }
    }

    public class ServiceAttribute<TService> : ServiceAttribute
    {
        public ServiceAttribute(ServiceLifetime lifetime, bool requireAutoLoadAttribute, object? tag = null) : base(lifetime, typeof(TService), requireAutoLoadAttribute, tag)
        {
        }
    }
}
