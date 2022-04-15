using Guppy;
using Guppy.Attributes;
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
    internal sealed class ServiceInitializer : GuppyInitializer<IServiceLoader>
    {
        protected override void Initialize(AssemblyHelper assemblies, IServiceCollection services, IEnumerable<IServiceLoader> loaders)
        {
            foreach(IServiceLoader loader in loaders)
            {
                loader.ConfigureServices(services);
            }
        }
    }
}
