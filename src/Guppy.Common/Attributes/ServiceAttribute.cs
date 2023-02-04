using Guppy.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Attributes
{
    public class ServiceAttribute : InitializableAttribute
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

        protected override bool ShouldInitialize(IServiceCollection services, Type classType)
        {
            var result =  base.ShouldInitialize(services, classType);

            if(this.RequireAutoLoadAttribute)
            {
                result &= classType.HasCustomAttribute<AutoLoadingAttribute>();
            }

            return result;
        }

        protected override void Initialize(IServiceCollection services, Type classType)
        {
            services.ConfigureCollection(sm =>
            {
                sm.AddService(classType)
                    .SetImplementationType(this.ServiceType ?? classType)
                    .SetLifetime(this.Lifetime);
            });
        }
    }

    public class ServiceAttribute<TService> : ServiceAttribute
    {
        public ServiceAttribute(ServiceLifetime lifetime, bool requireAutoLoadAttribute) : base(lifetime, typeof(TService), requireAutoLoadAttribute)
        {
        }
    }
}
