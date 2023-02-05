using Guppy.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Attributes
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

        protected override bool ShouldInitialize(GuppyEngine engine, Type classType)
        {
            var result =  base.ShouldInitialize(engine, classType);

            if(this.RequireAutoLoadAttribute)
            {
                result &= classType.HasCustomAttribute<AutoLoadAttribute>();
            }

            return result;
        }

        protected override void Initialize(GuppyEngine engine, Type classType)
        {
            engine.Services.ConfigureCollection(sm =>
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
