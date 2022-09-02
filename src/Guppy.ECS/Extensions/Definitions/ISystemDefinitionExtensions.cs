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
            IFilter? filter;

            foreach (Type filterType in definition.Filters)
            {
                ThrowIf.Type.IsNotAssignableFrom<IFilter>(filterType);

                filter = (provider.GetService(filterType) ?? ActivatorUtilities.CreateInstance(provider, filterType)) as IFilter;
                
                if(filter?.Invoke() == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
