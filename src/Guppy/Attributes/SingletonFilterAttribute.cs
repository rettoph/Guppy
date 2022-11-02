using Guppy.Attributes;
using Guppy.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Attributes
{
    public sealed class SingletonFilterAttribute : FilterAttribute
    {
        public override void Initialize(IServiceCollection services, Type classType)
        {
            base.Initialize(services, classType);

            services.AddFilter(new SingletonFilter(classType));
        }
    }
}
