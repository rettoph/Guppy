using Autofac;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Extensions.System.Reflection;

namespace Guppy.Core.Common.Attributes
{
    public class ServiceAttribute(ServiceLifetime lifetime, Type[]? serviceType, ServiceRegistrationFlags registrationFlags, object? tag = null) : GuppyConfigurationAttribute
    {
        public readonly IEnumerable<Type> ServiceTypes = serviceType ?? [];
        public readonly ServiceLifetime Lifetime = lifetime;
        public readonly ServiceRegistrationFlags RegistrationFlags = registrationFlags;
        public readonly object? Tag = tag;

        public ServiceAttribute(ServiceLifetime lifetime, ServiceRegistrationFlags registrationFlags, object? tag = null) : this(lifetime, null, registrationFlags, tag)
        {

        }

        protected override bool ShouldConfigure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            var result = base.ShouldConfigure(boot, builder, classType);

            if (this.RegistrationFlags.HasFlag(ServiceRegistrationFlags.RequireAutoLoadAttribute))
            {
                result &= classType.HasCustomAttribute<AutoLoadAttribute>();
            }

            return result;
        }

        protected override void Configure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            var service = builder.RegisterType(classType);

            if (this.RegistrationFlags.HasFlag(ServiceRegistrationFlags.AsSelf))
            {
                service.As(classType);
            }

            foreach (Type serviceType in this.ServiceTypes)
            {
                ThrowIf.Type.IsNotAssignableFrom(serviceType, classType);

                service.As(serviceType);
            }

            if (this.RegistrationFlags.HasFlag(ServiceRegistrationFlags.AsImplementedInterfaces))
            {
                service.AsImplementedInterfaces();
            }


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

    public class ServiceAttribute<TService>(ServiceLifetime lifetime, ServiceRegistrationFlags registrationFlags, object? tag = null) : ServiceAttribute(lifetime, [typeof(TService)], registrationFlags, tag)
    {
    }
}
