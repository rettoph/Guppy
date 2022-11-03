using Guppy.Attributes;
using Guppy.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class GlobalScopeFilterAttribute : InitializableAttribute
    {
        public override void Initialize(IServiceCollection services, Type classType)
        {
            base.Initialize(services, classType);

            services.AddFilter(new GlobalScopeFilter(classType));
        }
    }
}
