using Guppy.Attributes;
using Guppy.Common;
using Guppy.ECS.Definitions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Definitions
{
    internal static class ISystemDefinitionExtensions
    {
        public static bool Filter(this ISystemDefinition definition, IServiceProvider provider)
        {
            foreach (IFilter<ISystemDefinition> filter in definition.Filters)
            {
                if(filter?.Invoke(provider, definition) == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
