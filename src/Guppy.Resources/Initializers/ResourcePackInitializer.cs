using Guppy.Attributes;
using Guppy.Common.Providers;
using Guppy.Initializers;
using Guppy.Loaders;
using Guppy.Resources.Constants;
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
    internal sealed class ResourcePackInitializer : IGuppyInitializer
    {
        public void Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<IGuppyLoader> loaders)
        {
            var definitions = assemblies.GetTypes<IResourcePackDefinition>().WithAttribute<AutoLoadAttribute>(false);

            foreach(Type definition in definitions)
            {
                services.AddResourcePack(definition);
            }

            services.AddSingleton<IResourcePackProvider, ResourcePackProvider>();

            services.AddSetting<string>(SettingConstants.CurrentLanguage, LanguageConstants.en_US, true, nameof(Guppy));

            services.AddTransient<IResourcePackTypeProvider, ResourcePackStringProvider>();
        }
    }
}
