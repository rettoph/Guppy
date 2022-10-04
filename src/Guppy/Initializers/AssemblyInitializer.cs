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
    internal sealed class AssemblyInitializer : GuppyInitializer<IAssemblyLoader>
    {
        protected override void Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<IAssemblyLoader> loaders)
        {
            foreach(IAssemblyLoader loader in loaders)
            {
                loader.ConfigureAssemblies(assemblies);
            }
        }
    }
}
