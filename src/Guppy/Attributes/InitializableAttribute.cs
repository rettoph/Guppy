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
        public InitializableAttribute()
        {
        }

        public virtual void Initialize(IServiceCollection services, Type classType)
        {

        }
    }
}
