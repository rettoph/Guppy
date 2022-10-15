using Guppy.Common;
using Guppy.Common.Providers;
using Guppy.ECS.Definitions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Filters
{
    internal sealed class GuppySystemFilter : IFilter<ISystemDefinition>
    {
        public readonly Type Type;

        public GuppySystemFilter(Type type)
        {
            ThrowIf.Type.IsNotAssignableFrom<IGuppy>(type);

            this.Type = type;
        }

        public bool Invoke(IServiceProvider provider, ISystemDefinition arg)
        {
            var guppy = provider.GetRequiredService<Faceted<IGuppy>>();

            if(guppy.Type is null)
            {
                return false;
            }

            var result = this.Type.IsAssignableFrom(guppy.Type);

            return result;
        }
    }
}
