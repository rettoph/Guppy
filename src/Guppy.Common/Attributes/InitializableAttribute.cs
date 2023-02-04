using Guppy.Attributes.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public abstract class InitializableAttribute : Attribute
    {
        public InitializableAttribute()
        {
        }

        public virtual bool TryInitialize(IServiceCollection services, Type classType)
        {
            if(this.ShouldInitialize(services, classType))
            {
                this.Initialize(services, classType);
                return true;
            }

            return false;
        }

        protected virtual bool ShouldInitialize(IServiceCollection services, Type classType)
        {
            return true;
        }

        protected abstract void Initialize(IServiceCollection services, Type classType);
    }
}
