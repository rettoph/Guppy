using Guppy.Attributes;
using Guppy.Common.Providers;
using Guppy.Initializers;
using Guppy.Loaders;
using Guppy.Resources.Definitions;
using Guppy.Resources.Providers;
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
            var definitions = assemblies.GetTypes<IResourceDefinition>().WithAttribute<AutoLoadAttribute>(false);

            foreach(Type definition in definitions)
            {
                services.AddResource(definition);
            }

            services.AddScoped<IResourceProvider, ResourceProvider>();
        }
    }
}
