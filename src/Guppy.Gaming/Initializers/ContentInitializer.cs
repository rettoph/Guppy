using Guppy.Attributes;
using Guppy.Gaming.Definitions;
using Guppy.Gaming.Providers;
using Guppy.Gaming.Services;
using Guppy.Initializers;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using Minnow.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Initializers
{
    internal sealed class ContentInitializer : IGuppyInitializer
    {
        public void Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<IGuppyLoader> loaders)
        {
            var contents = assemblies.GetAttributes<ContentDefinition, AutoLoadAttribute>().Types;

            foreach (Type content in contents)
            {
                services.AddContent(content);
            }

            services.AddSingleton<IContentProvider, ContentProvider>();
        }
    }
}
