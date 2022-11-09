using Guppy.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class InitializableAttribute : Attribute
    {
        protected virtual ServiceLifetime DefaultServiceLifetime => ServiceLifetime.Scoped;

        public InitializableAttribute()
        {
        }

        public virtual bool ShouldInitialize(IServiceCollection services, Type classType)
        {
            if(services.Any(x => x.ServiceType == classType))
            {
                return true;
            }

            if(classType.IsAbstract)
            {
                return false;
            }

            if(classType.IsInterface)
            {
                return false;
            }

            if(classType.IsClass)
            {
                services.Add(ServiceDescriptor.Describe(classType, classType, this.DefaultServiceLifetime));

                return true;
            }

            return false;
        }

        public virtual void Initialize(IServiceCollection services, Type classType)
        {

        }
    }
}
