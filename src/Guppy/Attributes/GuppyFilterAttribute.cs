using Guppy.Attributes;
using Guppy.Common;
using Guppy.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Attributes
{
    public sealed class GuppyFilterAttribute : FilterAttribute
    {
        public readonly Type GuppyType;

        public GuppyFilterAttribute(Type guppyType)
        {
            ThrowIf.Type.IsNotAssignableFrom<IGuppy>(guppyType);

            this.GuppyType = guppyType;
        }

        public override void Initialize(IServiceCollection services, Type classType)
        {
            services.AddFilter(new GuppyFilter(classType, this.GuppyType));
        }
    }
}
