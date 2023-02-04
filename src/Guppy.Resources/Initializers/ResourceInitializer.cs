using Guppy.Attributes;
using Guppy.Attributes.Common;
using Guppy.Common.Providers;
using Guppy.Initializers;
using Guppy.Loaders;
using Guppy.Resources.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Initializers
{
    internal sealed class ResourceInitializer : IGuppyInitializer
    {
        public void Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<IGuppyLoader> loaders)
        {
            var packLoaders = assemblies.GetTypes<IPackLoader>().WithAttribute<AutoLoadAttribute>(true);

            foreach (var packLoader in packLoaders)
            {
                services.AddSingleton(typeof(IPackLoader), packLoader);
            }
        }
    }
}
