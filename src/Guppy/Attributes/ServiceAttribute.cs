using Guppy.Common.DependencyInjection;
using Guppy.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

        protected override bool ShouldConfigure(GuppyConfiguration builder, Type classType)
        {
            var result =  base.ShouldConfigure(builder, classType);

            if(this.RequireAutoLoadAttribute)
            {
                result &= classType.HasCustomAttribute<AutoLoadAttribute>();
            }

            return result;
        }

        protected override void Configure(GuppyConfiguration configuration, Type classType)
        {
            configuration.Services.ConfigureCollection(sm =>
            {
                sm.AddService(this.ServiceType ?? classType)
                    .SetImplementationType(classType)
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
