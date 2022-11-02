using Guppy.Attributes;
using Guppy.Common.Providers;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Initializers
{
    [AutoLoad]
    internal sealed class FilterInitializer : IGuppyInitializer
    {
        public void Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<IGuppyLoader> loaders)
        {
            var typesFilters = assemblies.GetAttributes<FilterAttribute>(true);

            foreach((Type type, FilterAttribute[] filters) in typesFilters)
            {
                foreach(var filter in filters)
                {
                    filter.Initialize(services, type);
                }
            }
        }
    }
}
